using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{


	public Camera[] extraCams;
	public static readonly int NUMBERCAMTYPES = 20;
	public int camSetting;
	

	public static readonly float INITIALZOOM = -50;
	private float currentZoom = -100;
	// Use this for initialization
	void Start ()
	{
		
		extraCams = GetComponentsInChildren<Camera>();
		allReset();
		
	}

	public void switchCam(int camSettingGiven)
	{
		camSetting = camSettingGiven;
		//first turns all of them off
		
		allReset();
		List<Camera> cams;
		switch (camSetting)
		{
			
			case 0:
				break;
			case 1:
				rotateOtherCams(.7f, 2, circleAroundCenter(2, 50));
				break;
			case 2:
				rotateOtherCams(.6f, 2, camerasBetweenZPoints(-150, -250, 12));
				break;
			case 3:
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						extraCams[i * 4 + j].enabled = true;
						GameObject g = extraCams[i * 4 + j].gameObject;
						g.transform.position = 
							new Vector3(0, 0,
								currentZoom - (i * 30));
						
						g.transform.rotation = Quaternion.Euler(0, 0, j * 90);
					}
				}
				
				break;
			case 4:
				rotateOtherCams(.1f, 4, zRotateAroundCenter(8));
				break;
			case 5:
				circleAroundCenter(8, currentZoom * 1.5f);
				Debug.Log(Vector3.Distance(extraCams[0].gameObject.transform.position, Vector3.zero));
				break;
			case 6:
				circleAroundCenter(15, currentZoom * 2);
				Debug.Log(Vector3.Distance(extraCams[0].gameObject.transform.position, Vector3.zero));
				break;
			case 7:
				zRotateAroundCenter(8);
				break;
			case 8:
				rotateOtherCams(.7f, 8, zRotateAroundCenter(9));
				break;
			case 9:
				rotateHalfCams(.5f, 9, zRotateAroundCenter(9));
				break;
			case 10:
				zRotateAroundCenter(11);
				break;
			case 11:
				cams = setupCamRange(0, 8);
				incrementFoV(cams, currentZoom * .7f, currentZoom * 1.3f );
				break;
			case 12:
				rotateOtherCams(.1f, 12, zRotateAroundCenter(11));
				break;
			case 13:
				zRotateAroundCenter(16);
				break;
			case 14:
				rotateHalfCams(.5f, 14, zRotateAroundCenter(15));
				break;
			case 15:
				rotateOtherCams(1f, 15, zRotateAroundCenter(17));
				break;
			case 16:
				zRotateAroundCenter(32);
				break;
			case 17:
				zRotateAroundCenter(64);
				break;
			case 18:
				zRotateAroundCenter(128);
				break;
			
			
			case 20:

				break;
				
		}

		int counter = 0;
		foreach (Camera c in extraCams)
		{
			if (c.enabled)
			{
				counter++;
			}
		}
		Debug.Log(camSetting + " camsetting; cameras on: " + counter);
	}

	public void zoom(float zoomMult)
	{
		currentZoom *= zoomMult;
		//take the exceptions out here
		if (camSetting != 500)
		{
			foreach (Camera cam in extraCams)
			{
				cam.gameObject.transform.position *= zoomMult;
			}
		}
	}

	public void setCurrentZoom(float zoom)
	{
		currentZoom = zoom;
	}


	public void allReset()
	{
		foreach (Camera cam in extraCams)
		{
			cam.enabled = false;
			cam.fieldOfView = 60;
			cam.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
			cam.gameObject.transform.position = new Vector3(0, 0, currentZoom);
			cam.nearClipPlane = .3f;
			cam.farClipPlane = 20000;
		}
	}

	//inclusive to both
	public List<Camera> setupCamRange(int index1, int index2)
	{
		List<Camera> cams = new List<Camera>();
		for (int i = index1; i < index2 + 1; i++)
		{
			cams.Add(extraCams[i]);
			extraCams[i].enabled = true;
		}
		return cams;
	}
	
	//number disncludes default camera
	public List<Camera> zRotateAroundCenter(int nOfCams)
	{
		List<Camera> cams = new List<Camera>();
		for (int i = 0; i < nOfCams; i++)
		{
			cams.Add(extraCams[i]);
			extraCams[i].enabled = true;
			GameObject g = extraCams[i].gameObject;
			g.transform.position =
				new Vector3(g.transform.position.x, g.transform.position.y, currentZoom);
//			Debug.Log(currentZoom);
			g.transform.rotation = Quaternion.Euler(0, 0, i * (360.0f / nOfCams));			
		}
		return cams;
	}
	
	public List<Camera> camerasBetweenZPoints(float z1, float z2, int nOfCams)
	{
		List<Camera> cams = new List<Camera>();
		float increment = (z2 - z1) / nOfCams;
		for (int i = 0; i < nOfCams; i++)
		{
			cams.Add(extraCams[i]);
			extraCams[i].enabled = true;
			GameObject g = extraCams[i].gameObject;
			g.transform.position = new Vector3(0, 0, z1 + i * increment);
		}
		return cams;
	}

	public List<Camera> circleAroundCenter(int nOfCams, float distFromCenter)
	{
		List<Camera> cams = new List<Camera>();
		float step = 2 * Mathf.PI / nOfCams;
		int indexCounter = 0;
		for (float theta = 0; theta < 2 * Mathf.PI; theta += step)
		{
			float x = 0 - distFromCenter * Mathf.Cos(theta);
			float z = 0 - distFromCenter * Mathf.Sin(theta);
			cams.Add(extraCams[indexCounter]);
			extraCams[indexCounter].enabled = true;
			GameObject g = extraCams[indexCounter].gameObject;
			g.transform.position = new Vector3(x, 0, z);
			g.transform.LookAt(Vector3.zero);
			indexCounter++;
		}
		return cams;
	}

	public void incrementFoV(List<Camera> cams, float v1, float v2)
	{
		float increment = (v2 - v1) /  cams.Count;
		for (int i = 0; i < cams.Count; i++)
		{
			cams[i].fieldOfView = v1 + i * increment;
		}
	}

	public IEnumerator camRotate( float roSpeed, int camSet, Camera cam)
	{
		
		float currentZRotation = 0;
		
		while (camSetting == camSet)
		{
			
			GameObject g = cam.gameObject;
			g.transform.rotation = Quaternion.Euler(g.transform.position.x, g.transform.position.y, 
				currentZRotation);
			currentZRotation += roSpeed;
			
			yield return new WaitForSeconds(0);
		}
	}

	public void rotateHalfCams(float speed, int caseNumber, List<Camera> cams)
	{
		for (int i = 0; i < cams.Count; i++)
		{
			if (i < cams.Count / 2)
			{
				StartCoroutine(camRotate(speed, caseNumber, cams[i]));
			}
			else
			{
				StartCoroutine(camRotate(-speed, caseNumber, cams[i]));
			}
		}
	}
	
	public void rotateOtherCams(float speed, int caseNumber, List<Camera> cams)
	{
		for (int i = 0; i < cams.Count; i++)
		{
			if (i % 2 == 0)
			{
				StartCoroutine(camRotate(speed, caseNumber, cams[i]));
			}
			else
			{
				StartCoroutine(camRotate(-speed, caseNumber, cams[i]));
			}
		}
	}
	
	
}


















/*
public abstract class CamManObj
{
	public List<CamContainer> cams;
	public bool objOn;
	public bool relativeZoom;

	public CamManObj(List<Camera> cams)
	{
		foreach (Camera cam in cams)
		{
			CamContainer cCont = new CamContainer(cam);
		}
	}

	public void camZoom(float currentZoom)
	{
		foreach (CamContainer cC in cams)
		{
			//linear zoom
			if (!relativeZoom)
			{
				float nextZoom = currentZoom + (cC.initialDist - CameraManager.INITIALZOOM);
			}
		}
	}

	public void camObjOn()
	{
		return;
	}

	public void camObjOff()
	{
		return;
	}

	public class CamContainer
	{

		public CamContainer(Camera c)
		{
			camObj = c.gameObject;
			cam = c;
			initialDist = Vector3.Distance(c.gameObject.transform.position, Vector3.zero);
		}
		public GameObject camObj { get; set; }
		public Camera cam { get; set; }
		public float initialDist { get; set; }
	}
}*/


/*

how to update zoom? there would have to be an update script in camera manager
it could be specific for each case, or there could be a switch statement that is based on the camera zoom 
instruction or something

//no could just be an update zoom method, switch in there. then could do an if statement, but that seemsshitty
because it would be two separate things rather than one. The problem is that once the switch is done, the cameras 
are set, and the only thing that continues is the switch number, and the appropriate ienums which 
are also done in this weird two part way of checking the type. How should it be done so that you dont
have to keep checking the type, and configuring everything around this? it seems the appropriate way 
would be for each camera setting to be an object with an interface update zoom, and its own update or 
ienum in there. each object would have its own camera list with its own data, and the overall one would just be 
looking at the current object and assigning its cameras to that.
With this: I would have a List of these objects, and just create them here rather than a switch.
okay this is clearly better: how to do the Ienums? Maybe each object has a flag obj on or obj off,
and when it switches to the next one it just goes obj off. great idea! :)




*/

