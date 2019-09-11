using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseController : MonoBehaviour {

	
	public Texture2D cursorTexture;
	public Texture2D transparentPixel;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;

	public Vector3 cursorSavedPos = Vector3.zero;
	public bool mouseGone;
	

	private Camera camera;
	// Use this for initialization
	void Start ()
	{
		camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		cursorOn();
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(cursorSavedPos, Input.mousePosition) > .1f)
		{
			if (mouseGone)
			{
				cursorOn();
			}
		} else {
			StartCoroutine(cursorCheck());
		}
		//Debug.Log(cursorSavedPos + "saved; and current: " + Input.mousePosition);
		cursorSavedPos = Input.mousePosition;
	}
	
	void cursorOn()
	{
		Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
		mouseGone = false;
	}

	void cursorOff()
	{
		Debug.Log("CUURSOR OFF");
		Cursor.SetCursor(transparentPixel, Vector2.zero, cursorMode);
		mouseGone = true;
	}

	public IEnumerator cursorCheck()
	{
		
		Vector3 cursorPos = Input.mousePosition;
		yield return new WaitForSecondsRealtime(2);
		if (!mouseGone && Vector3.Distance(cursorPos, Input.mousePosition) <= .1f)
		{
			cursorOff();
		}
	
	}
	
	
	
}
