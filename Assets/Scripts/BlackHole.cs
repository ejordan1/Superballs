using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackHole : MonoBehaviour
{
	private bool gravCD;
	public float bMass = 100;
	public float bExp = 2;
	public int blackHoleType;
	public MeshRenderer myMesh;
	public MeshRenderer childMesh;
	public bool inHiding;
	
	// Use this for initialization
	void Start ()
	{
		myMesh = GetComponent<MeshRenderer>();
		childMesh = transform.GetChild(0).GetComponent<MeshRenderer>();
		
		
		if (BouncyShoot.blackHoles.Count > 12)
		{
			blackHoleType = 0;
		}
		else
		{
			blackHoleType = 1;
		}
		updateBHRender();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!gravCD)
		{
			StartCoroutine(applyGrav(Static.bHIncrement));
			
		}
	}

	public void blackHoleGravity()
	{
		if (blackHoleType != 0){
			foreach (BallClass ballClass in BouncyShoot.balls)
			{
				Vector3 directionAdded = transform.position - ballClass.ball.transform.position;
				if (blackHoleType == 2)
				{
					directionAdded = -directionAdded;
				}

				if (Static.toggleOldMath)
				{
					float promixityMeasure =
						Static.blackHoleRadius - directionAdded.magnitude; //.normalize to add magnitude is velocity.
					if (Static.blackHoleRadius > directionAdded.magnitude)
					{//instead save it somewhere here, then the last thing in bouncyshoot update is to go through all balls and addforce
						ballClass.totalForceToAdd += directionAdded.normalized *
						                             (Mathf.Pow(promixityMeasure + Static.gAddedPre, Static.gExp) *
						                              (Static.gMult) + Static.gAddedPost);
						
						/*ballClass.rB.AddForce(directionAdded.normalized *
						                       (Mathf.Pow(promixityMeasure + Static.gAddedPre, Static.gExp) *
						                        (Static.gMass) + Static.gAddedPost));*/
					}
				}
				else
				{

					float currentMass = bMass;
					float currentExp = bExp;
					if (Static.massOverride)
					{
						currentMass = Static.gMult;
					}
					if (Static.expOverride)
					{
						currentExp = Static.gExp;
					}

					ballClass.totalForceToAdd += (directionAdded.normalized *
					                       (currentMass /
					                        Mathf.Pow(Vector3.Distance(ballClass.ball.transform.position, transform.position),
						                        currentExp)));

				}

			}
		} //child of camera, moving
	}

	public IEnumerator applyGrav(float timeIncrement)
	{
		gravCD = true;
		blackHoleGravity();
		yield return new WaitForSeconds(timeIncrement);
		gravCD = false;
	}
	
	//cannot change black hole type
	
	/*void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0))
		{
			blackHoleType++;
			updateBHRender();
		}
	}*/

	void updateBHRender()
	{
		if (!inHiding)
		{
			switch (blackHoleType % 3)
			{
				case (0):
				{
					childMesh.enabled = true;
					myMesh.enabled = false;
					break;
				}
				case (1):
				{
					childMesh.enabled = false;
					myMesh.enabled = true;
					break;
				}
				case (2):
				{
					childMesh.enabled = true;
					myMesh.enabled = true;
					break;
				}
			}
		}

	}

	public void hideBH()
	{
		inHiding = !inHiding;
		updateHiding();
	}

	public void updateHiding()
	{
		if (inHiding)
		{
			childMesh.enabled = false;
			myMesh.enabled = false;
		}
		else
		{
			updateBHRender();
		}
		
	}
}


//the universe size is really just the black hole radius. dont get confused and make this over complicated