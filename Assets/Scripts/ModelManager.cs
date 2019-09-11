using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefaultNamespace;
//using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ModelManager : MonoBehaviour
{
	private Collider col;
	public static Dictionary<string, List<Vector3>> modelDict;
	public static BouncyShoot bouncyShootref;
	public Text currentModelText;
	public GameObject modelsParent;

	public static int childCount; //terrible code
	// Use this for initialization

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Comma))
		{
			switchModelDown();
		}
		if (Input.GetKeyDown(KeyCode.Period))
		{
			switchModel();
		}
	}

	void Awake ()
	{
		modelDict = new Dictionary<string, List<Vector3>>();
		currentModelText = GameObject.Find("CurrentModelText").GetComponent<Text>();
	modelsParent = GameObject.Find("ModelsParent");
		childCount = modelsParent.transform.childCount;
		for (int i = 0; i < modelsParent.transform.childCount; i++)
		{
			GameObject model = modelsParent.transform.GetChild(i).gameObject;
			modelDict[model.name] = new List<Vector3>();
			model.SetActive(true);
			
			
			for (int u = -100; u < 100; u++)
			{
				for (int j = -100; j < 100; j++)
				{
					for (int k = -100; k < 100; k++)
					{
					
						Vector3 point = new Vector3(u, j, k);

						if (Physics.CheckSphere(point, 1f))
						{
							modelDict[model.name].Add(point);
						}
					}
				}
			}
			model.SetActive(false);
		}
	}

	[ContextMenu("ToolsWrite file")]
	public void WriteString()
	{
		string path = "Assets/Resources/vects.txt";

		//Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(path, true);
		writer.WriteLine("Test asdf asdf asdf asdf asdf asdf asdf asdf ");
		writer.Close();

		//Re-import the file to update the reference in the editor
		//AssetDatabase.ImportAsset(path); 
		//TextAsset asset = Resources.Load("vects") as TextAsset;

		//Print the text from the file
		//Debug.Log(asset.text);
	}

	public static List<BallClass> fireModel(GameObject modelParent)
	{
		List<BallClass> ballStructs = new List<BallClass>();
		foreach (Vector3 point in modelDict.ElementAt(Static.modelIndex % childCount).Value)
		{
				
			BallClass ballS = bouncyShootref.ballFire(bouncyShootref.gameObject, point, Color.clear);
			ballS.ball.transform.parent = modelParent.transform;
			ballStructs.Add(ballS);
				
		}
		return ballStructs;
	}
	public void switchModelDown()
	{
		
		Static.modelIndex--;
		if (Static.modelIndex < 0)
		{
			Static.modelIndex += modelDict.Count;
		}
		currentModelText.text = modelDict.ElementAt(Static.modelIndex % modelsParent.transform.childCount).Key;
		for (int i = 0; i < BouncyShoot.balls.Count; i++)
		{
			BallClass bs = BouncyShoot.balls[i];
			bs.relativeStartPos = modelDict.ElementAt(Static.modelIndex % childCount)
				.Value[i % modelDict.ElementAt(Static.modelIndex % childCount).Value.Count];
			
		}

		/*int n = shapeVects.length - ballList.Legnth;
		if (n > 0){
//going down from the remaining ones
			for (int i = shapeVects.length; i < n; i--){
				ballFire newBall, relativeVect is shapeVects[i];
			}
		}*/

		
		
	}
	
	public void switchModel()
	{
		
		Static.modelIndex++;

		currentModelText.text = modelDict.ElementAt(Static.modelIndex % modelsParent.transform.childCount).Key;
		for (int i = 0; i < BouncyShoot.balls.Count; i++)
		{
			BallClass bs = BouncyShoot.balls[i];
			bs.relativeStartPos = modelDict.ElementAt(Static.modelIndex % childCount)
				.Value[i % modelDict.ElementAt(Static.modelIndex % childCount).Value.Count];
			
		}

		/*int n = shapeVects.length - ballList.Legnth;
		if (n > 0){
//going down from the remaining ones
			for (int i = shapeVects.length; i < n; i--){
				ballFire newBall, relativeVect is shapeVects[i];
			}
		}*/

		
		
	}

}
