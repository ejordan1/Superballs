using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BouncyShoot : MonoBehaviour
{

	private CameraManager cameraManager;
	private GeometryLaunchManager geometryLaunchManager;
	private Gatling gatling;
	
	private float cameraZoomToCenterSpd =  .1f; //DONT CHANGE THESE HERE
	private float lastBlackOpacity = .05f;
	public float spaceAccel;
	public float spaceMult;
	public GameObject camTarget;
	
	
	private int cameraSetting = 0;
	
	public GameObject neptunePrefab;
	public GameObject spherePrefab;
	public GameObject cubePrefab;
	
	public static readonly Color colorZero = Color.clear;

	private List<Vector3> camStartPositions;
	
	public static List<BallClass> balls;
	public static Dictionary<GameObject, BlackHole> blackHoles;
	public static Dictionary<GameObject, List<BallClass>> parentObjects;

	public List<GameObject> ballPrefabs;
	public GameObject canvas;
	public GameObject BallInstYesPrefab;
	public GameObject BallInstNoPrefab;
	public GameObject blackHolePrefab;
	public GameObject walls;
	public PhysicMaterial physicMat;
	public GameObject wall;
	public Material ballDefaultMat;
	public Material ballTransparentMat;
	public Material nonInstancedMat;

	public GameObject camObj;
	public GameObject shooter2;
	public Camera camera;
	public GameObject ballParent;
	public Vector3 mousePos;
	public float rtMult = 100;
	public float upMult = 100;
	private bool ballOnCool;
	private AudioSource audio;
	public GameObject mountains;
	public Image blackScreenImage;

	public DragMouseOrbit dragMouseOrbitRef;
	public MouseLook mouseLookRef;


	public AudioClip[] bounceSounds;

	private Slider ballSizeSlider;
	private Slider bounceinessSlider;
	private Slider ballForceSlider;
	private Slider ballVelocitySlider;
	private Slider frictionSlider;
	private Slider wallSlider;
	private Slider ballMassSlider;
	private Slider gAddedPreSlider;
	
	private Slider gAddedPostSlider;
	private Slider gMultSlider;
	private Slider gExpSlider;
	private Slider BHIncrementSlider;
	private Slider BlackHoleRadiusSlider;
	private Slider ballSeperatenessSlider;
	private Slider bHSeperatenessSlider;
	private Slider timeStepSlider;

	private InputField ColorRInput;
	private InputField ColorGInput;
	private InputField ColorBInput;
	private InputField ColorAInput;
	private InputField BallMetalicInput;
	private InputField BallSmoothInput;
	private InputField blackOpacityInput;
	
	private InputField d1InputField;
	private InputField d2InputField;
	private InputField d3InputField;
	private InputField bHd1InputField;
	private InputField bHd2InputField;
	private InputField bHd3InputField;

	private Toggle wallsToggle;
	private Toggle blackHoleToggle;
	private Toggle physicsLayerCollisionToggle;
	private Toggle gravityToggle;
	private Toggle mountainsToggle;
	private Toggle expOverrideToggle;
	private Toggle massOverrideToggle;
	private Toggle oldMathToggle;
	private Toggle velocityDirectToggle;


	private Text ballSizeText;
	private Text ballForceText;
	private Text ballVelocityText;
	private Text bouncinessText;
	private Text frictionText;
	private Text ballMassText;
	private Text gAddedPreText;
	
	private Text gAddedPostText;
	private Text gMultText;
	private Text gExpText;
	
	private Text wallZPosText;
	private Text BHIncrementText;
	private Text BlackHoleRadiusText;
	private Text frameRateText;
	private Text ballCountText;
	private Text ballSeperatenessText;
	private Text bHSeparatenessText;
	private Text timeStepText;
	private Text vertModelText;

	
	// Use this for initialization
	void Start ()
	{
		cameraManager = GameObject.Find("ExtraCams").GetComponent<CameraManager>();
		geometryLaunchManager = GetComponent<GeometryLaunchManager>();
		gatling = GetComponent<Gatling>();
		BallInstYesPrefab = spherePrefab;
		blackHoles = new Dictionary<GameObject, BlackHole>();
		parentObjects = new Dictionary<GameObject, List<BallClass>>();
		ImageCreater.bouncyShootRef = this;
		ModelManager.bouncyShootref = this;
		MeshVertManager.bouncyShootRef = this;
		GeometryLaunchManager.bouncyShootRef = this;
		gatling.bouncyShootRef = this;
		
		//wait how is this working: why are the others static but also are there
		
		
		canvas = GameObject.Find("Canvas");
		d1InputField = GameObject.Find("MatrixD1").GetComponent<InputField>();
		d2InputField = GameObject.Find("MatrixD2").GetComponent<InputField>();
		d3InputField = GameObject.Find("MatrixD3").GetComponent<InputField>();
		
		bHd1InputField = GameObject.Find("BHMatrixD1").GetComponent<InputField>();
		bHd2InputField = GameObject.Find("BHMatrixD2").GetComponent<InputField>();
		bHd3InputField = GameObject.Find("BHMatrixD3").GetComponent<InputField>();

		ColorRInput = GameObject.Find("BallColorR").GetComponent<InputField>();
		ColorGInput = GameObject.Find("BallColorG").GetComponent<InputField>();
		ColorBInput = GameObject.Find("BallColorB").GetComponent<InputField>();
		ColorAInput = GameObject.Find("BallColorA").GetComponent<InputField>();
		BallMetalicInput = GameObject.Find("BallMetallic").GetComponent<InputField>();
		BallSmoothInput = GameObject.Find("BallSmoothness").GetComponent<InputField>();
		blackOpacityInput = GameObject.Find("BlackOpacity").GetComponent<InputField>();
		
		//set spacemult
		//set accelmult
		GameObject.Find("AccelSlider").GetComponent<Slider>().value = 0.4f;
		GameObject.Find("AccelMult").GetComponent<Slider>().value = 0.008f;

		
		ballSeperatenessText = GameObject.Find("BallSeperatenessText").GetComponent<Text>();
		mountains = GameObject.Find("Mountains");
		ballParent = new GameObject("MiscBallParent");
		walls = GameObject.Find("Walls");
		camObj = GameObject.Find("Main Camera");
		mouseLookRef = camObj.GetComponent<MouseLook>();
		dragMouseOrbitRef = camObj.GetComponent<DragMouseOrbit>();
		balls = new List<BallClass>();
		ballSizeText = GameObject.Find("BallSizeText").GetComponent<Text>();
		ballForceText = GameObject.Find("BallForceText").GetComponent<Text>();
		ballVelocityText = GameObject.Find("BallVelocityText").GetComponent<Text>();
		//bouncinessText = GameObject.Find("BouncinessText").GetComponent<Text>();
		//frictionText = GameObject.Find("FrictionText").GetComponent<Text>();
		ballMassText = GameObject.Find("BallMassText").GetComponent<Text>();
		//wallZPosText = GameObject.Find("WallZPosText").GetComponent<Text>();
		gAddedPreText = GameObject.Find("GAddedPreText").GetComponent<Text>();
		vertModelText = GameObject.Find("VertModelText").GetComponent<Text>();
		gAddedPostText = GameObject.Find("GAddedPostText").GetComponent<Text>();
		gMultText = GameObject.Find("GMultText").GetComponent<Text>();
		gExpText = GameObject.Find("GExpText").GetComponent<Text>();
		//bDistText = GameObject.Find("BDistText").GetComponent<Text>();
		
		BHIncrementText = GameObject.Find("BHIncrementText").GetComponent<Text>();
		BlackHoleRadiusText = GameObject.Find("BlackHoleRadiusText").GetComponent<Text>();
		frameRateText = GameObject.Find("FrameRate").GetComponent<Text>();
		ballCountText = GameObject.Find("BallCount").GetComponent<Text>();
		bHSeparatenessText = GameObject.Find("BHSeparatenessText").GetComponent<Text>();
		timeStepText = GameObject.Find("TimeStepText").GetComponent<Text>();
		
		audio = GetComponent<AudioSource>();
		wall = GameObject.Find("Wall");
		
		
		camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		

		camStartPositions = new List<Vector3>();
		camStartPositions.Add(camera.transform.position);
		
		
		
		
		ballSizeSlider = GameObject.Find("BallSize").GetComponent<Slider>();
		ballForceSlider = GameObject.Find("BallForce").GetComponent<Slider>();
		ballVelocitySlider = GameObject.Find("BallVelocity").GetComponent<Slider>();
		//bounceinessSlider = GameObject.Find("Bounciness").GetComponent<Slider>();
		//frictionSlider = GameObject.Find("Friction").GetComponent<Slider>();
		ballMassSlider = GameObject.Find("BallMass").GetComponent<Slider>();
//		wallSlider = GameObject.Find("WallZPos").GetComponent<Slider>();
		gAddedPreSlider = GameObject.Find("GAddedPre").GetComponent<Slider>();
		gAddedPostSlider = GameObject.Find("GAddedPost").GetComponent<Slider>();
		gMultSlider = GameObject.Find("GMult").GetComponent<Slider>();
		gExpSlider = GameObject.Find("GExp").GetComponent<Slider>();
		
		//bDistSlider = GameObject.Find("BDist").GetComponent<Slider>();
		BHIncrementSlider = GameObject.Find("BHIncrement").GetComponent<Slider>();
		//wallsToggle = GameObject.Find("WallToggle").GetComponent<Toggle>();
		//blackHoleToggle = GameObject.Find("BlackHoleToggle").GetComponent<Toggle>();
		//physicsLayerCollisionToggle = GameObject.Find("LayerCollisionToggle").GetComponent<Toggle>();
		//gravityToggle = GameObject.Find("GravityToggle").GetComponent<Toggle>();
		BlackHoleRadiusSlider = GameObject.Find("BlackHoleRadius").GetComponent<Slider>();
		//mountainsToggle = GameObject.Find("MountainsToggle").GetComponent<Toggle>();
		//massOverrideToggle = GameObject.Find("MassOverrideToggle").GetComponent<Toggle>();
		//expOverrideToggle = GameObject.Find("ExpOverrideToggle").GetComponent<Toggle>();
		ballSeperatenessSlider = GameObject.Find("BallSeperateness").GetComponent<Slider>();
		bHSeperatenessSlider = GameObject.Find("BHSeparateness").GetComponent<Slider>();
		oldMathToggle = GameObject.Find("OldMathToggle").GetComponent<Toggle>();
		velocityDirectToggle = GameObject.Find("VelocityDirectToggle").GetComponent<Toggle>();
		timeStepSlider = GameObject.Find("TimeStep").GetComponent<Slider>();
		blackScreenImage = GameObject.Find("BlackOpacityImage").GetComponent<Image>();
		
		ballSizeSlider.value = Static.ballSize;
		//bounceinessSlider.value = Static.bounciness;
		ballForceSlider.value = Static.ballForce;
		ballVelocitySlider.value = Static.ballVelocity;
	//	frictionSlider.value = Static.friction;
		//wallSlider.value = Static.wallZPos;
		ballMassSlider.value = Static.ballMass;
		gAddedPreSlider.value = Static.gAddedPre;
		gAddedPostSlider.value = Static.gAddedPost;
		gMultSlider.value = Static.gMult;
		gExpSlider.value = Static.gExp;
		
		BlackHoleRadiusSlider.value = Static.blackHoleRadius;
		ballSeperatenessSlider.value = 1;
	//	frictionSlider.value = Static.friction;
		BHIncrementSlider.value = Static.bHIncrement;
		velocityDirectToggle.isOn = Static.velocityDirectToggle;
		timeStepSlider.value = 1;   //TIME STEP RESETS
		
	
		//wall.transform.position = new Vector3(150, 48.3f, wallSlider.value);
		BallInstYesPrefab.GetComponent<Rigidbody>().mass = ballMassSlider.value;
//		ball.GetComponent<Rigidbody>().drag = frictionSlider.value;


		//wallsToggle.isOn = Static.toggleWalls;
		//blackHoleToggle.isOn = Static.toggleBHGravity;
		//gravityToggle.isOn = Static.toggleGravity;
		//physicsLayerCollisionToggle.isOn = Static.toggleCollision;
		//mountainsToggle.isOn = Static.toggleMountains;
		//massOverrideToggle.isOn = Static.massOverride;
		//expOverrideToggle.isOn = Static.expOverride;
		oldMathToggle.isOn = Static.toggleOldMath;
		d1InputField.text = Static.matrixD1.ToString();
		d2InputField.text = Static.matrixD2.ToString();
		d3InputField.text = Static.matrixD3.ToString();
		
		bHd1InputField.text = Static.bHmatrixD1.ToString();
		bHd2InputField.text = Static.bHmatrixD2.ToString();
		bHd3InputField.text = Static.bHmatrixD3.ToString();
		toggleOldMath();
		
		toggleVelocityDirect();
		recreateBH();

		Static.ballColorR = 190;
		Static.ballColorG = 190;
		Static.ballColorB = 190;
		
		updateBallColor();
		
		
		
		gExpSlider.value = .04f;
		BallSmoothInput.text = "1.5";
		BallMetalicInput.text = "1";
		timeStepSlider.value = .4f;
		//BHIncrementSlider.value = .5f;


	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.E))
		{
			resetCams();
		}
		
		//test
		if (Input.GetKeyDown(KeyCode.U))
		{
			if (Static.gMult != 0)
			{
				gMultSlider.value = 0;
			}
			else
			{
				gMultSlider.value = 30;
			}
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			if (Static.gMult != 0)
			{
				gMultSlider.value = 0;
			}
			else
			{
				gMultSlider.value = 500;
			}
		}

		
		if (Input.GetKeyDown(KeyCode.N))
		{
			fireNeptune(this.gameObject, Vector3.zero);
		}
		if (Input.GetKeyDown(KeyCode.Semicolon))
		{
			if (timeStepSlider.value != 0)
			{
				timeStepSlider.value = 0;
			}
			else
			{
				timeStepSlider.value = .7f;
			}
		}

		if (Input.GetKeyDown(KeyCode.H))
		{
			Static.currentBallShape++;
			BallInstYesPrefab = ballPrefabs[Static.currentBallShape % ballPrefabs.Count];

		}
		
		
		
		if (Input.GetKey(KeyCode.B))
		{ 
		
			
			Quaternion a = Quaternion.LookRotation(new Vector3(50, 0, 0));
			Quaternion b = Quaternion.LookRotation(new Vector3(50, 0, -25));
			Quaternion c = Quaternion.Inverse(a) * b;
//			Debug.Log(Quaternion.Inverse(a).eulerAngles);
//			Debug.Log(a.eulerAngles + ", " + b.eulerAngles + ", " + c.eulerAngles);
		
			//Debug.Log(q * v);
		}
		

		

		if (Input.GetKeyDown(KeyCode.RightBracket))
		{
			toggleSuperMode();
		}
		
		if (Input.GetKeyDown(KeyCode.Backslash))
		{
			//if already in supermode0
			if (Static.blackOpacity == 0)
			{
				blackOpacityInput.text = lastBlackOpacity.ToString();
			}
			else
			//if not in supermode0 make it supermode0
			{
				//keeps track of last
				lastBlackOpacity = Static.blackOpacity;
				//sets it to super
				blackOpacityInput.text = "0";
			}
		}
		/*if (Input.GetKeyDown(KeyCode.Slash))
		{
			if (Static.friction == 0)
			{
				frictionSlider.value = .3f;
			}
			else
			{
				frictionSlider.value = 0;
			}
		}*/
		

		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1))
		{
			Static.ballColorR = 190;
			Static.ballColorG = 190;
			Static.ballColorB = 190;
			
			updateBallColor();
		}
		
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2))
		{
			Static.ballColorR = 0;
			Static.ballColorG = 0;
			Static.ballColorB = 245;
			
			updateBallColor();
		}
		
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha3))
		{
			Static.ballColorR = 160;
			Static.ballColorG = 0;
			Static.ballColorB = 0;
			
			updateBallColor();
		}
		
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha4))
		{
			Static.ballColorR = 0;
			Static.ballColorG = 140;
			Static.ballColorB = 0;
			
			updateBallColor();
		}
		
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha5))
		{
			Static.ballColorR = 0;
			Static.ballColorG = 200;
			Static.ballColorB = 200;
			
			updateBallColor();
		}
		
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha6))
		{
			Static.ballColorR = 50;
			Static.ballColorG = 0;
			Static.ballColorB = 200;
			
			updateBallColor();
		}
		
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha7))
		{
			Static.ballColorR = 0;
			Static.ballColorG = 140;
			Static.ballColorB = 0;
			
			updateBallColor();
		}
		
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha8))
		{
			Static.ballColorR = 0;
			Static.ballColorG = 140;
			Static.ballColorB = 0;
			
			updateBallColor();
		}
		
		


		mousePos = camera.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 1));
		
		transform.LookAt(mousePos);

		
		if (Input.GetKeyDown(KeyCode.G))
		{
			GameObject matrixParent = new GameObject("matrixParent");
			List<BallClass> MatrixList = new List<BallClass>();
			
			for (int i = 0; i < Static.matrixD1; i++)
			{
				for (int j = 0; j < Static.matrixD2; j++)
				{
					for (int h = 0; h < Static.matrixD3; h++)
					{
						BallClass ballS = ballFire(this.gameObject, new Vector3(i * Static.ballSize * Static.ballSeperatness - Static.ballSize * Static.ballSeperatness * ((Static.matrixD1 - 1) / 2),
							j * Static.ballSize * Static.ballSeperatness - Static.ballSize * Static.ballSeperatness * ((Static.matrixD2 - 1)/ 2),
							h * Static.ballSize * Static.ballSeperatness), colorZero);
						ballS.ball.transform.parent = matrixParent.transform;
						MatrixList.Add(ballS);
					}
				}
			}
			parentObjects[matrixParent] = MatrixList;
		}
		
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(0);
		}
		
		//redo this with pooling
		if (Input.GetKeyDown(KeyCode.T))
		{
			foreach (BallClass bs in balls)
			{
				foreach (List<BallClass> list in parentObjects.Values)
				{
					if (list.Contains(bs))
					{
						list.Remove(bs);
					}
				}
				Destroy(bs.ball);
			}
			balls.Clear();
		}
		
		if (Input.GetKeyDown(KeyCode.I))
		{
			if (Static.imageDivideBy != 0)
			{
				GameObject parent = new GameObject();
				parent.transform.eulerAngles = new Vector3(Time.deltaTime, 0f, 0f);
				
				
				parent.transform.position = transform.position;
			
				parentObjects[parent] = ImageCreater.fireImage(Static.currentImage, Static.imageDivideBy, parent);


				Vector3 midPoint = calculateMidPoint(parentObjects[parent]);
				foreach (BallClass ball in parentObjects[parent])
				{
					ball.relativePosToCenter = transform.position - midPoint;
				}
			}
		}
		
		if (Input.GetKeyDown(KeyCode.L))
		{
			GameObject parent = new GameObject();
			parent.transform.position = transform.position;
			parentObjects[parent] = ModelManager.fireModel(parent);
			
			rotateParent(parent, transform.rotation);
			
			Vector3 midPoint = calculateMidPoint(parentObjects[parent]);
			foreach (BallClass ball in parentObjects[parent])
			{
				ball.relativePosToCenter = transform.position - midPoint; //SHOULD NOT BE DONE DURING RUNTIME,
				//SHOULD NOT BE RELATIVE TO LAUNCH ORIENTATION OF SHAPE
			}
			
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			GameObject parent = new GameObject();
			parent.transform.position = transform.position;
			parentObjects[parent] = MeshVertManager.fireVertModel(parent);
			
			rotateParent(parent, transform.rotation);
			
			Vector3 midPoint = calculateMidPoint(parentObjects[parent]);
			foreach (BallClass ball in parentObjects[parent])
			{
				ball.relativePosToCenter = transform.position - midPoint; //SHOULD NOT BE DONE DURING RUNTIME,
				//SHOULD NOT BE RELATIVE TO LAUNCH ORIENTATION OF SHAPE
			}
		}

		
		//BUG: the reason the space bar doesn't work is because their relative position
		//to center is calculated only once and not after they are created.
		//this is something to keep in mind to reorganize all of it
		if (Input.GetKeyDown(KeyCode.S))
		{
			GameObject parent = new GameObject();
			parent.transform.position = transform.position;
			parentObjects[parent] = GeometryLaunchManager.fireCircle(parent, 200, 10);
			
			rotateParent(parent, transform.rotation);
			
			Vector3 midPoint = calculateMidPoint(parentObjects[parent]);
			foreach (BallClass ball in parentObjects[parent])
			{
				ball.relativePosToCenter = transform.position - midPoint; //SHOULD NOT BE DONE DURING RUNTIME,
				//SHOULD NOT BE RELATIVE TO LAUNCH ORIENTATION OF SHAPE
			}
		}
		
		if (Input.GetKeyDown(KeyCode.A))
		{
			GameObject parent = new GameObject();
			parent.transform.position = transform.position;
			parentObjects[parent] = geometryLaunchManager.fireCircleDoubleCone(parent);
			
			rotateParent(parent, transform.rotation);
			
			Vector3 midPoint = calculateMidPoint(parentObjects[parent]);
			foreach (BallClass ball in parentObjects[parent])
			{
				ball.relativePosToCenter = transform.position - midPoint; //SHOULD NOT BE DONE DURING RUNTIME,
				//SHOULD NOT BE RELATIVE TO LAUNCH ORIENTATION OF SHAPE
			}
		}

		if (Input.GetKeyDown( KeyCode.B))
		{
			gatling.firingOn(0, true);
			gatling.firingOn(.33f, true);
			gatling.firingOn(.66f, true);
			//gatling.firingOn(.99f, true);
		}
		if (Input.GetKeyUp(KeyCode.B))
		{
			gatling.firingOff();
		}
		if (Input.GetKeyDown( KeyCode.M))
		{
			gatling.firingCircleToggle();
		}
		if (Input.GetKeyUp(KeyCode.M))
		{
			gatling.firingCircleToggle();
		}

		if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Minus))
		{
			if (Static.vertModelIndex == 0)
			{
				Static.vertModelIndex = MeshVertManager.vertModelDict.Count;
				
			}
			Static.vertModelIndex--;
			vertModelText.text = 
				MeshVertManager.vertModelDict[Static.vertModelIndex].name;
			
		}
		if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Equals))
		{
			Static.vertModelIndex++;
			vertModelText.text = 
				MeshVertManager.vertModelDict[Static.vertModelIndex 
				                              % MeshVertManager.vertModelDict.Count].name;
		}
		
		//garbage code bad bad bad
		var d = Input.GetAxis("Mouse ScrollWheel");
		Vector3 zoomTarg = Vector3.zero;
		float distToTarg;
		if (camTarget != null)
		{
			zoomTarg = camTarget.transform.position;
		}
		distToTarg = Vector3.Distance(transform.position, zoomTarg);
		if (d > 0f && distToTarg > 1)
		{
				camObj.transform.position = camObj.transform.position * (1 - cameraZoomToCenterSpd);
			cameraManager.zoom(.99f);
		}
		else if (d < 0f && distToTarg < 5000)
		{
				camObj.transform.position = camObj.transform.position * (1 + cameraZoomToCenterSpd);
			cameraManager.zoom(1.01f);
		}

		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Equals))
		{
//			Debug.Log("LEFT SHFIT");
			/*if (Static.timeStep > .03f)
			{
				timeStepSlider.value += .0006f;
			}
			else
			{
				timeStepSlider.value += .0001f;
			}*/
			if (timeStepSlider.value < 1)
			timeStepSlider.value = timeStepSlider.value * 1.003f;
		}
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Minus))
		{
			/*if (Static.timeStep > .03f)
			{
				timeStepSlider.value -= .0006f;
			}
			else
			{
				timeStepSlider.value -= .0001f;
			}*/
			timeStepSlider.value = timeStepSlider.value * .997f;
		}
// chill its fine 

		//colors 1234s
		//default 190 190 190
		//0 0 245
		//160 0 0 
		//0 140 0 
			
			
		if (Input.GetKey(KeyCode.X))
		{
			for (int i = 0; i < 10; i++)
			{
				ballFire(this.gameObject, Vector3.zero, colorZero);
			}
		}
		
		if (Input.GetKeyDown(KeyCode.O))
		{
			
			if (!canvas.activeSelf)
			{
				canvas.SetActive(true);
			}
			else
			{

				canvas.SetActive(false);
			}

		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			foreach (BlackHole bh in blackHoles.Values)
			{
				bh.hideBH();
			}
		}

		if (Input.GetKey(KeyCode.F))
		{
			if (!ballOnCool)
			{
				
				StartCoroutine(ballCD(Static.ballCoolTime));
			}
		}
		if (Input.GetKeyDown(KeyCode.K))
		{
			sparkAway();
		}

		frameRateText.text = 1 / Time.deltaTime + "";
		ballCountText.text = balls.Count + "";

		if (Input.GetKeyDown(KeyCode.Quote))
		{
			cameraSetting++;
			cameraManager.switchCam(cameraSetting % CameraManager.NUMBERCAMTYPES);
		}
		
		if (Input.GetKeyDown(KeyCode.Z))
		{
			if (camera.nearClipPlane == .3f)
			{
				camera.nearClipPlane = 99999;
			}
			else
			{
				camera.nearClipPlane = .3f;
			}
		}
		
	}

	void FixedUpdate()
	{

		if (Input.GetKey(KeyCode.J))
		{
			sparkIn();
		}
		if (Input.GetKey(KeyCode.Space))
		{
			returnToParentPosition();
		}
		if (Input.GetKey(KeyCode.V))
		{
			returnToAllPosition();
		}
		

		addBallForces();
		
		//delete extra parent objects
		for (int i = parentObjects.Count - 1; i > -1; i--)
		{
			//Debug.Log("CHECKED PARENT " + i + ", count: " + parentObjects.ElementAt(i).Value.Count);
			if (parentObjects.ElementAt(i).Value.Count == 0)
			{
				parentObjects.Remove(parentObjects.ElementAt(i).Key);
				
			}
		}
	}

	public void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			mouseLookRef.setRotationToCurrent();
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			mouseLookRef.setRotationToCurrent();
		}
		
		if (Input.GetKey(KeyCode.UpArrow))
		{
			camObj.transform.position *= .997f;
			cameraManager.zoom(.997f);
			
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			camObj.transform.position *= 1.003f;
			cameraManager.zoom(1.003f);
			
		}

		
		if (Input.GetKey(KeyCode.Q))
		{
			if (Static.blackOpacity <= 1)
			{
				Static.blackOpacity *=  1.01f;
			}

			if (Static.blackOpacity < .01f)
			{
				Static.blackOpacity = .01f;
			}
			
			updateBlackOpacity();
		}
		if (Input.GetKey(KeyCode.W))
		{
			if (Static.blackOpacity > .01f)
			{
				Static.blackOpacity *=  .99f;
			}
			updateBlackOpacity();
		}


		if (Input.GetMouseButtonDown(1))
		{
			mouseLookRef.setRotationToCurrent();
		}
		if (Input.GetMouseButton(1))
		{
			Vector3 targetPos = Vector3.zero;
			float distance;
			if (camTarget)
			{
				targetPos = camTarget.transform.position;
				distance = Vector3.Distance(camObj.transform.position, camTarget.transform.position);
			}
			else
			{
				distance = Vector3.Distance(camObj.transform.position, Vector3.zero);
			}
		
			if (Input.GetKey(KeyCode.LeftShift))
			{
				
				dragMouseOrbitRef.updateMouseOrbit(distance, targetPos);
			}
			else
			{
				mouseLookRef.updateMouseLook();
			}
		}
		
		//bad bad code
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			Vector3 targetPos = Vector3.zero;
			float distance;
			if (camTarget)
			{
				targetPos = camTarget.transform.position;
				distance = Vector3.Distance(camObj.transform.position, camTarget.transform.position);
			}
			else
			{
				distance = Vector3.Distance(camObj.transform.position, Vector3.zero);
			}
		
				
				dragMouseOrbitRef.rotateLeft(distance, targetPos);

		}
		
		if (Input.GetKey(KeyCode.RightArrow))
		{
			Vector3 targetPos = Vector3.zero;
			float distance;
			if (camTarget)
			{
				targetPos = camTarget.transform.position;
				distance = Vector3.Distance(camObj.transform.position, camTarget.transform.position);
			}
			else
			{
				distance = Vector3.Distance(camObj.transform.position, Vector3.zero);
			}
		
				
			dragMouseOrbitRef.rotateRight(distance, targetPos);

		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			foreach (BallClass bC in balls)
			{
				bC.bS.colorChangeToggle();
			}
		}
	}

	//badly written code: shouldn't get static.ballForce, you should pass this in
	public BallClass ballFire(GameObject shootingObj, Vector3 addedVect, Color color)
	{
		//audio.Play ();
//		print (mousePos);
		//get camera position, add something to it in forward direction, then add the x and y position in respect to that.

		
		//HOTFIX CHANGE THIS BACK: THIS MAKES IT SO ALL COLORS ARE WHITE
		//if (color != Color.white)
		{
		//	color = Color.white;
		}


		GameObject newBall;
		//true is here to always make it a gpu instanced no-color ball because the images he sent
		//were not true black 
		if (color == Color.clear  || color == colorZero || color == Color.black)
		{
			newBall = Instantiate(BallInstYesPrefab, shootingObj.transform.position + addedVect, Quaternion.identity);
		}
		else
		{
			newBall = Instantiate(BallInstNoPrefab, shootingObj.transform.position + addedVect, Quaternion.identity);
		}
		BallClass ballClass = new BallClass();
		ballClass.ball = newBall;
		ballClass.bS = newBall.GetComponent<BallScript>();
		ballClass.rB = newBall.GetComponent<Rigidbody>();
		ballClass.relativeStartPos = addedVect;
		
		balls.Add(ballClass);

		
		//false is here to always make it a gpu instanced no-color ball because the images he sent
		//were not true black. up there its true and here its false because the statements ar ereverse
		if (color != colorZero  && color != Color.black)
		{
			newBall.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
		}
		else
		{
		//check box for this
			//obj.GetComponent<MeshRenderer>().material.SetColor("_Color", Random.ColorHSV());
		}
		if (Static.velocityDirectToggle)
		{
			ballClass.rB.velocity = shootingObj.transform.forward * Static.ballVelocity;
		}
		else
		{
			Vector3 forceAddedVect = shootingObj.transform.forward * Static.ballForce;
			ballClass.totalForceToAdd += forceAddedVect;
		}
		int randomSound = Random.Range(0, bounceSounds.Length - 1);
		//obj.GetComponent<AudioSource>().clip = bounceSounds[randomSound];
		newBall.transform.localScale = new Vector3(Static.ballSize * newBall.transform.localScale.x, Static.ballSize * newBall.transform.localScale.y, Static.ballSize * newBall.transform.localScale.z);

		return ballClass;

	}

	public IEnumerator ballCD(float cdTime)
	{
		ballOnCool = true;
		ballFire(this.gameObject, Vector3.zero, colorZero);
		yield return new WaitForSeconds(cdTime);
		ballOnCool = false;
	}

	public void setBallSize()
	{
		
		Static.ballSize = ballSizeSlider.value;
		ballSizeText.text = "size = " + ballSizeSlider.value;
		Static.ballCoolTime = Static.ballSize / 20;
	}

	public void setBallForce()
	{
		
		Static.ballForce = ballForceSlider.value;
		ballForceText.text = "force: " + ballForceSlider.value;
	}
	
	public void setBallVelocity()
	{
		
		Static.ballVelocity = ballVelocitySlider.value;
		ballVelocityText.text = "velocity: " + ballVelocitySlider.value;
	}
	
	public void setBounciness()
	{
		Static.bounciness = bounceinessSlider.value;
		physicMat.bounciness = Static.bounciness;
		bouncinessText.text = "bouncy: " + Static.bounciness;
		
	}

	public void setSpaceAccel(Slider slider)
	{
		
		spaceAccel = slider.value;
		//Debug.Log(spaceAccel);
		
	}
	
	public void setSpaceMult(Slider slider)
	{
		
		spaceMult = slider.value;
		//Debug.Log(spaceMult);
		
	}

	/*public void setFriction()
	{
		Static.friction = frictionSlider.value;
		foreach (BallClass ballS in balls)
		{
			ballS.rB.drag = Static.friction;
		}
		BallInstYesPrefab.GetComponent<Rigidbody>().drag = frictionSlider.value;
		frictionText.text = "fric: " + Static.friction;
		
		Debug.Log(Static.friction + "= Friction");
	}*/

	public void setBallMass()
	{
		Static.ballMass = ballMassSlider.value;
		BallInstYesPrefab.GetComponent<Rigidbody>().mass = ballMassSlider.value;
		ballMassText.text = "mass: " + ballMassSlider.value;
	}

	public void setWallPos()
	{
		Static.wallZPos = wallSlider.value;
		if (wall != null)
		{
			wall.transform.position = new Vector3(150, 48.3f, wallSlider.value);
		}
		wallZPosText.text = "wall: " + wallSlider.value;
	}

	public void setGAddedPre()
	{
		Static.gAddedPre = gAddedPreSlider.value;
		gAddedPreText.text = "gPre: " + Static.gAddedPre;
	}

	public void setGAddedPost()
	{
		Static.gAddedPost = gAddedPostSlider.value;
		gAddedPostText.text = "gPost: " + Static.gAddedPost;
	}
	
	public void setGMult()
	{
		Static.gMult = gMultSlider.value;
		gMultText.text = "gMult: " + Static.gMult;
	}
	
	public void setGExp()
	{
		Static.gExp = gExpSlider.value;
		gExpText.text = "gExp: " + Static.gExp;
	}

	public void setBHIncrement()
	{
		Static.bHIncrement = BHIncrementSlider.value;
		BHIncrementText.text = "BHIncrement: " + Static.bHIncrement;
	}

	public void toggleWalls()
	{
		Static.toggleWalls = wallsToggle.isOn;
		walls.SetActive(Static.toggleWalls);
	}
	
	
	public void toggleLayerCollision()
	{
		Static.toggleCollision = physicsLayerCollisionToggle.isOn;
		Physics.IgnoreLayerCollision(8,8, !Static.toggleCollision);
	}
	
	public void toggleGravity()
	{
		Static.toggleGravity = gravityToggle.isOn;
		foreach (BallClass ballObj in balls)
		{
			
			ballObj.rB.useGravity = Static.toggleGravity;
		}
		BallInstYesPrefab.GetComponent<Rigidbody>().useGravity = Static.toggleGravity;
	}
	
	public void toggleMountains()
	{
		Static.toggleMountains = mountainsToggle.isOn;
		mountains.SetActive(Static.toggleMountains);

	}
	
	public void setBlackHoleRadius()
	{
		Static.blackHoleRadius = BlackHoleRadiusSlider.value;
		BlackHoleRadiusText.text = "BlackHoleRadiusSize: " + Static.blackHoleRadius;
	}
	
	public void toggleMassOverride()
	{
		Static.massOverride = massOverrideToggle.isOn;

	}
	
	public void toggleExpOverride()
	{
		Static.expOverride = expOverrideToggle.isOn;
	}
	
	public void inputR()
	{
		int.TryParse(ColorRInput.text, out Static.ballColorR);
		updateBallColor();
	}
	public void inputG()
	{
		int.TryParse(ColorGInput.text, out Static.ballColorG);
		updateBallColor();
	}
	public void inputB()
	{
		int.TryParse(ColorBInput.text, out Static.ballColorB);
		updateBallColor();
	}
	public void inputA()
	{
		float.TryParse(ColorAInput.text, out Static.ballColorA);
		if (Static.ballColorA == 1)
		{
			//BallInstYesPrefab.GetComponent<MeshRenderer>().material = ballDefaultMat;
			updateBallColor();
		}
		else
		{
			//BallInstYesPrefab.GetComponent<MeshRenderer>().material = ballTransparentMat;
			updateBallColor();
		}
	}

	public void inputMetallic()
	{
		float.TryParse(BallMetalicInput.text, out Static.ballMetallic);
		ballDefaultMat.SetFloat("_Metallic", Static.ballMetallic);
		nonInstancedMat.SetFloat("_Metallic", Static.ballMetallic);
	}

	//not working?
	public void inputSmooth()
	{
		float.TryParse(BallSmoothInput.text, out Static.ballSmoothness);
		Debug.Log(Static.ballSmoothness + "smoothness");
		ballDefaultMat.SetFloat("_Glossiness", Static.ballSmoothness);
		nonInstancedMat.SetFloat("_Glossiness", Static.ballMetallic);
	}
	
	public void inputBlackOpacity()
	{
		float.TryParse(blackOpacityInput.text, out Static.blackOpacity);
		updateBlackOpacity();
	}

	public void updateBlackOpacity()
	{
		blackScreenImage.color = new Color(0,0,0, Static.blackOpacity);
//		Debug.Log(Static.blackOpacity);
		blackOpacityInput.text = Static.blackOpacity.ToString();
	}


	public void updateBallColor()
	{
//		Debug.Log(Static.ballColorR + ", " + Static.ballColorG + ", " + Static.ballColorB + ", " + Static.ballColorA);
		ballDefaultMat.SetColor("_Color", new Color(Static.ballColorR / 255f, Static.ballColorG / 255f, Static.ballColorB /  255f, Static.ballColorA));
		ballTransparentMat.SetColor("_Color", new Color(Static.ballColorR / 255f, Static.ballColorG / 255f, Static.ballColorB /  255f, Static.ballColorA));

		
	}

	public void inputD1()
	{
		 int.TryParse(d1InputField.text, out Static.matrixD1);
	}
	public void inputD2()
	{
		int.TryParse(d2InputField.text, out Static.matrixD2);
	}
	public void inputD3()
	{
		int.TryParse(d3InputField.text, out Static.matrixD3);
	}

	public void changeSeperateness()
	{
		Static.ballSeperatness = ballSeperatenessSlider.value;
	}
	
	public void changebHSeperateness()
	{
		Static.bHSeparateness = bHSeperatenessSlider.value;
		recreateBH();
	}

	public void inputBHMatrix1()
	{
		int.TryParse(bHd1InputField.text, out Static.bHmatrixD1);
		recreateBH();
	}
	
	public void inputBHMatrix2()
	{
		int.TryParse(bHd2InputField.text, out Static.bHmatrixD2);
		recreateBH();
	}
	
	public void inputBHMatrix3()
	{
	
		int.TryParse(bHd3InputField.text, out Static.bHmatrixD3);
		recreateBH();
	}

	public void toggleOldMath()
	{
		Static.toggleOldMath = oldMathToggle.isOn;
		gAddedPreSlider.gameObject.SetActive(oldMathToggle.isOn);
		gAddedPostSlider.gameObject.SetActive(oldMathToggle.isOn);
		BlackHoleRadiusSlider.gameObject.SetActive(oldMathToggle.isOn);
	}

	public void toggleVelocityDirect()
	{
		Static.velocityDirectToggle = velocityDirectToggle.isOn;
		ballForceSlider.gameObject.SetActive(!Static.velocityDirectToggle);
		ballVelocitySlider.gameObject.SetActive(Static.velocityDirectToggle);
	}

	public void setTimeStep()
	{
		Static.timeStep = timeStepSlider.value;
		Time.timeScale = Static.timeStep;
		//Time.fixedDeltaTime = 1 / Static.timeStep;

	}

	public void addBallForces()
	{
		foreach (BallClass ball in balls)
		{
			ball.rB.AddForce(ball.totalForceToAdd);
			ball.totalForceToAdd = Vector3.zero;
		}
	}

	public void resetCams()
	{
		camObj.transform.position = camStartPositions[0];
		camObj.transform.LookAt(Vector3.zero);
		cameraManager.setCurrentZoom(-100);
	}

	public void recreateBH()
	{
		foreach (GameObject bh in blackHoles.Keys)
		{
			Destroy(bh);
		}
		blackHoles.Clear();

		for (int i = 0; i < Static.bHmatrixD1; i++)
		{
			for (int j = 0; j < Static.bHmatrixD2; j++)
			{
				for (int h = 0; h < Static.bHmatrixD3; h++)
				{
					GameObject newBH = GameObject.Instantiate(blackHolePrefab,
						new Vector3(i * Static.bHSeparateness - (Static.bHmatrixD1 - 1) * Static.bHSeparateness / 2,
							j * Static.bHSeparateness - (Static.bHmatrixD2 - 1) * Static.bHSeparateness / 2,
							h * Static.bHSeparateness), Quaternion.identity);
					blackHoles.Add(newBH, newBH.GetComponent<BlackHole>());
				}
			}
		}
		
		
	}

	public void rotateParent(GameObject parent, Quaternion rotation)
	{
		
		parent.transform.rotation = rotation;
	}
	
	
	//NOTE: the fact that it continues to move is a real challenge. how to solve this? 
	public void returnToParentPosition()
	{
		foreach (GameObject parent in parentObjects.Keys)
		{
			Vector3 midPoint = calculateMidPoint(parentObjects[parent]);
		
			//have access to original positions, so can find original vext through that
			Vector3 currentVect = Vector3.zero;
			Vector3 originalVect = Vector3.zero;
			/*for (int i = 0; i < 10; i++)
			{

				BallClass bc = parentObjects[parent][i * 50];
				originalVect += bc.relativePos;
				currentVect += (bc.ball.transform.position - midPoint); //not normalized to the original size. problem?
			}*/
			BallClass bc = parentObjects[parent][0];
			originalVect += bc.relativePosToCenter;
			//THE ORIGINAL VECT IS NOT IN RESPECT TO MIDPOINT
			currentVect += (bc.ball.transform.position - midPoint); //not normalized to the original size. problem?
			
			Quaternion q1 = Quaternion.LookRotation(originalVect);
			Quaternion q2 = Quaternion.LookRotation(currentVect);
			
			Quaternion q3 = Quaternion.Inverse(q1) * q2;
			
			//Debug.Log(bc.relativeStartPos + "ov, cv " + currentVect + ", " + q1.eulerAngles + ", " + q2.eulerAngles + ", " + q3.eulerAngles);
			
			
		//	Debug.Log(q3.eulerAngles + " euler angles");
			//Debug.Log(currentRotate + " is current rotation");

			//this is not that complicated dont get caught up
			foreach (BallClass bStruct in parentObjects[parent])
			{
				//directionToAddwithBallSeparateness
				Vector3 relativeStartWithBallSep = new Vector3(bStruct.relativeStartPos.x * Static.ballSeperatness, 
					bStruct.relativeStartPos.y * Static.ballSeperatness,
					bStruct.relativeStartPos.z * Static.ballSeperatness);
				//where does position of current rotate go in here
				Vector3 directionToAdd =  (q3 * relativeStartWithBallSep - ((bStruct.ball.transform.position - midPoint)));

				
				if (Vector3.Distance(bStruct.ball.transform.position, midPoint) > .01f)
				{ //						5								
					Vector3 shouldVelocity = 5 * (directionToAdd).normalized * Mathf.Pow(directionToAdd.magnitude, spaceAccel); //square it

					
					
					
					//closer it is, the more it should be similar to the should velocity:
					Vector3 forceAdded = spaceMult * ((shouldVelocity - bStruct.rB.velocity).normalized *
					                              //this is the acceleration: the difference between should vect and current
					                              Mathf.Pow(Vector3.Distance(shouldVelocity, bStruct.rB.velocity), 2) /
					                              Mathf.Pow(Mathf.Clamp(Vector3.Distance(midPoint, transform.position), .3f, 500),
						                              1 / 3));
					
					//totally hacky solution: clamp the force added:
					if (forceAdded.magnitude > 3000)
					{
						forceAdded = forceAdded.normalized * 3000;
					}
					bStruct.totalForceToAdd += forceAdded;
				}
			}
		}
	}
	
	//not in use
	public void returnToParentPosition2()
	{
		foreach (GameObject parent in parentObjects.Keys)
		{
			Vector3 midPoint = calculateMidPoint(parentObjects[parent]);
		
			//have access to original positions, so can find original vext through that
			Vector3 currentVect = Vector3.zero;
			Vector3 originalVect = Vector3.zero;
			/*for (int i = 0; i < 10; i++)
			{

				BallClass bc = parentObjects[parent][i * 50];
				originalVect += bc.relativePos;
				currentVect += (bc.ball.transform.position - midPoint); //not normalized to the original size. problem?
			}*/
			BallClass bc = parentObjects[parent][0];
			originalVect += bc.relativePosToCenter;
			//THE ORIGINAL VECT IS NOT IN RESPECT TO MIDPOINT
			currentVect += (bc.ball.transform.position - midPoint); //not normalized to the original size. problem?
			
			Quaternion q1 = Quaternion.LookRotation(originalVect);
			Quaternion q2 = Quaternion.LookRotation(currentVect);
			
			Quaternion q3 = Quaternion.Inverse(q1) * q2;
			
			//Debug.Log(bc.relativeStartPos + "ov, cv " + currentVect + ", " + q1.eulerAngles + ", " + q2.eulerAngles + ", " + q3.eulerAngles);
			
			
		//	Debug.Log(q3.eulerAngles + " euler angles");
			//Debug.Log(currentRotate + " is current rotation");

			//this is not that complicated dont get caught up
			foreach (BallClass bStruct in parentObjects[parent])
			{
				
				//where does position of current rotate go in here
				Vector3 directionToAdd =  (q3 * bStruct.relativeStartPos - ((bStruct.ball.transform.position - midPoint)));

				
				if (Vector3.Distance(bStruct.ball.transform.position, midPoint) > .2f)
				{ //						5
					Vector3 shouldVelocity = 5 * (directionToAdd).normalized * Mathf.Pow(directionToAdd.magnitude, .4f); //square it

					float accel = .4f; //.4
					//closer it is, the more it should be similar to the should velocity:
					Vector3 forceAdded = accel * ((shouldVelocity - bStruct.rB.velocity).normalized *
					                              //this is the acceleration: the difference between should vect and current
					                              Mathf.Pow(Vector3.Distance(shouldVelocity, bStruct.rB.velocity), 2) /
					                              Mathf.Pow(Mathf.Clamp(Vector3.Distance(midPoint, transform.position), .3f, 500),
						                              1 / 3));
					bStruct.totalForceToAdd += forceAdded;
				}
			}
		}
	}
	
	//take ten random balls, calculate total original vect (do this at beginning?), then each frame calculate the updated orientation
	//also add in the velocity of the whole thing somehow

	//V multplier is X (after space mult) times stronger than the space bar
	//the problem with v right now is that it calculates what velocity it should be at, and then
	//adds force to get it there, but it doesn't keep adding force, it just does to get it to the should place.
	//seems like ideally it would keep adding force or something
	public void returnToAllPosition()
	{
		Vector3 midPoint = calculateMidPoint(balls);

		foreach (BallClass bStruct in balls)
		{

			Vector3 directionToAdd = (bStruct.relativeStartPos - (bStruct.ball.transform.position - midPoint));

			if (Vector3.Distance(bStruct.ball.transform.position, midPoint) > .2f)
			{
				Vector3 shouldVelocity = 3 * (directionToAdd).normalized * Mathf.Pow(directionToAdd.magnitude, .4f); //square it

				//float accel = .4f;
				//maybe should have its own variable not spaceMult
				//closer it is, the more it should be similar to the should velocity:
				Vector3 forceAdded = spaceMult * 5 * ((shouldVelocity - bStruct.rB.velocity).normalized *
				                              //this is the acceleration: the difference between should vect and current
				                              Mathf.Pow(Vector3.Distance(shouldVelocity, bStruct.rB.velocity), 2) /
				                              Mathf.Pow(Mathf.Clamp(Vector3.Distance(midPoint, transform.position), .3f, 5000),
					                              1 / 3));
				bStruct.totalForceToAdd += forceAdded;
			}
		}
	}

	public void sparkIn()
	{
		foreach (BallClass bStruct in balls)
		{
			bStruct.totalForceToAdd += bStruct.ball.transform.position.normalized * -100;
			
		}
	}

	public void sparkAway()
	{
		foreach (BallClass bStruct in balls)
		{
			bStruct.totalForceToAdd += bStruct.ball.transform.position.normalized * 20000;
		}
	}

	public Vector3 calculateMidPoint(List<BallClass> ballsList)
	{
		Vector3 totalVect = Vector3.zero;
		for (int i = ballsList.Count - 1; i > -1; i--)
		{
			if (ballsList[i].ball != null)
			{
				totalVect += ballsList[i].ball.transform.position;
			}
			else
			{
				ballsList.Remove(ballsList[i]); //this may be bad: should remove from everywhere
			}
		}
			
			

		//calculate new midpoint, then orientation
		return (totalVect / ballsList.Count);
	}

	public void toggleSuperMode()
	{
		Static.toggleSuperMode = !Static.toggleSuperMode;
		updateClearMode();
	}
	


	public void updateClearMode()
	{
		//fix this; make it so only black or super mode
		
		if (Static.toggleSuperMode)
		{
			camera.clearFlags = CameraClearFlags.Depth;
		} else if (Static.toggleSolidColor)
		{
			camera.clearFlags = CameraClearFlags.SolidColor;
		}
	}

	public BallClass fireNeptune(GameObject shootingObj, Vector3 addedVect)
	{

		GameObject neptune;
		
		neptune = Instantiate(neptunePrefab, shootingObj.transform.position + addedVect, Quaternion.identity);
	
		BallClass ballClass = new BallClass();
		ballClass.ball = neptune;
		ballClass.bS = neptune.GetComponent<BallScript>();
		ballClass.rB = neptune.GetComponent<Rigidbody>();
		ballClass.relativeStartPos = addedVect;
		
		//optional: to add neptune to other balls list or not? have to because it loops through to apply the force. 
		//this system is not good because it makes all the balls have to work within the same force system
		balls.Add(ballClass);

		//should the vect be normalized for the regular ballfire?
		
		Vector3 forceAddedVect = (shootingObj.transform.forward) * 40 * shootingObj.transform.position.magnitude;
		Debug.Log(shootingObj.transform.forward.normalized);
			ballClass.totalForceToAdd += forceAddedVect;
		
		return ballClass;

	}

}
//velocity minus (center minus ballpoint


//speed
//current get to target speed
//current get to target speed can be -
//calculates: as long as it can get to target within (get to target speed is distance * exponent) then its good
//so if distance is really small then get to target time is small, if large then it is larger
//get to target is just: distance / speed: time 