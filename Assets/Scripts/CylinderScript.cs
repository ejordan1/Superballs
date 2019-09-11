using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderScript : MonoBehaviour
{
	private int counter;

	public bool picasso;
	private float str = .02f;

	public Quaternion targetRotation;
	// Use this for initialization
	void Start ()
	{
		counter = (int)Random.Range(0, 100);
		if (picasso)
		{
			transform.rotation = Quaternion.Euler(transform.position);
		}
		else
		{
			transform.rotation = targetRotation = Quaternion.LookRotation(Vector3.zero - transform.position);	
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		//this was sick! using position for rotation coordinates
		/*Vector3 ro = -transform.position;
		transform.rotation = Quaternion.Euler(ro.x, ro.y, ro.z);*/
		/*if (counter % 30 == 0)
		{
			if (!picasso)
			{
				//targetRotation = Quaternion.LookRotation(Vector3.zero - transform.position);	
			}
			else
			{
				targetRotation = Quaternion.Euler(transform.position);
			}
			
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
		
		}
		counter++;*/
	}
}
