using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.Experimental.UIElements.Button;

public class ImageManager : MonoBehaviour
{
	private Object[] images;
	private Text currentImageText;
	private InputField DivideByInput;
	
	// Use this for initialization
	void Start ()
	{
		DivideByInput = GameObject.Find("ImageDivideBy").GetComponent<InputField>();
		images = Resources.LoadAll("Textures", typeof(Texture2D));
		currentImageText = GameObject.Find("CurrentImageText").GetComponent<Text>();
		
		foreach (Object t in images)
		{
			//Debug.Log(t.name);
		}

		DivideByInput.text = Static.imageDivideBy.ToString();
		Static.currentImage = (Texture2D)images[(Static.imgIndex % images.Length)];
		currentImageText.text = images[Static.imgIndex].name;
	
	}	
	
	// Update is called once per frame
	void Update () {
		
	}

	public void switchImage()
	{
		
		
		Static.imgIndex++;
		//Debug.Log(Static.imgIndex % images.Length + "asdf");
		currentImageText.text = images[Static.imgIndex % images.Length].name + "  =>";
		Static.currentImage = (Texture2D)images[Static.imgIndex % images.Length];
		
	}

	public void dividePixelsBy()
	{
		 int.TryParse(DivideByInput.text, out Static.imageDivideBy);
	}
}
