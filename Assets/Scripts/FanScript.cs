using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class FanScript : MonoBehaviour
{
	private float strength = 100; //make this based on size in some way?
	
	//this and other code is to try to base strength on proximity to fan, but it doesn't work because the bottom of the fan is a point
	//private float exp = 1.5f;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Ball"))
		{
			BallClass bs = BouncyShoot.balls.FirstOrDefault(b => b.ball == other.gameObject);
			
			//SIMPLE AND WORKS: adds equal force regardless of position of ball in fan
			bs.rB.AddRelativeForce(transform.rotation * Vector3.up * strength);
			
			
			
			//adds force from the bottom of the fan
			float dist = Vector3.Distance(other.gameObject.transform.position, gameObject.transform.position + Vector3.down * (transform.localScale.y / 2));
			//if (dist < transform.localScale.y)
			//{
			//	bs.rB.AddForce(Vector3.up * transform.rotation * Mathf.Pow(transform.localScale.y - dist, exp) * strength);
			//}
		}
			
		
	}

	

}
