using UnityEngine;
using System.Collections;
using U3DXT.Core;

public class Main : MonoBehaviour {

	const float leftRotationLimit  = -.5f;
	const float rightRotationLimit = .5f;
	const float upRotationLimit    = -.3f;
	const float downRotationLimit = .3f;
	
	bool isGoingLeft = false;
	bool isGoingRight = false;
	bool isGoingUp = false;
	bool isGoingDown = false;
	bool isOpen = false;
	
	
	// Use this for initialization
	void Start () 
	{
		//openHand();
//		rotateArmToLeft();
		//rotateArmToRight();
		//rotateArmToUp();
		//rotateArmToDown();
		
		if (CoreXT.IsDevice) {
			OpenEars.Heard += delegate(object sender, OpenEarsHeardEventArgs e) {
				Log("Heard: " + e.Phrase);
				
				OnCommand(e.Phrase);
			};

			OpenEars.Init(new string[] {"LEFT", "RIGHT", "OPEN", "CLOSE", "CENTER"});
			OpenEars.StartListening();
			Log("Open Ears initialized.");
			
			
			MindWave.Sensed += delegate(object sender, MindWaveSensedEventArgs e) {
				Log("Sensed: " + e.Phrase);
				
				OnCommand(e.Phrase);
			};
			MindWave.Init();  // init for mind control
			MindWave.StartListening();
			Log("MindWave initialized.");
			
			Log("Screen size: " + Screen.width + "x" + Screen.height);
		}
	}
	
	void OnCommand(string cmd) {
		switch(cmd) {
		case "LEFT":
			rotateArmToLeft();
			break;
		case "RIGHT":
			rotateArmToRight();
			break;
		case "OPEN":
			openHand();
			break;
		case "CLOSE":
			closeHand();
			break;
		case "SET_LEFT":
			break;
		case "SET_RIGHT":
			break;
		case "SET_OPEN":
			break;
		case "SET_CLOSE":
			break;
		case "CENTER":
			GameObject.Find("Main Camera").camera.transform.position = new Vector3(0, 225, 50);
			break;
		}
	}
	
	void OnGUI() {
				
		OnGUILog();
	}
	
	// Update is called once per frame
	void Update () 
	{
		var arm = GameObject.FindWithTag("Player");
		
		//left
		if(isGoingLeft)
		{
			if(	arm.transform.rotation.y > leftRotationLimit)
			{
				rotateArmLeftRightBy(-2);
			}
			else
			{
				isGoingLeft = false;
				
			}
		}
		
		//right
		if(isGoingRight)
		{
			if(	arm.transform.rotation.y < rightRotationLimit)
			{
				rotateArmLeftRightBy(2);
			}
			else
			{
				isGoingRight = false;
				
			}
		}
		/*
		//up
		if(isGoingUp)
		{
			
			if(	arm.transform.rotation.x > upRotationLimit)
			{
				rotateArmUpDownBy(-1);
			
			}
			else
			{
				isGoingUp = false;
			}
			
		}
		
		//down
		if(isGoingDown)
		{
			if(	arm.transform.rotation.x < downRotationLimit)
			{
				rotateArmUpDownBy(1);
			
			}
			else
			{
				isGoingDown = false;
			}
			
		}
		*/
		

	}
	
	public void rotateArmLeftRightBy(int number)
	{
		GameObject.FindWithTag("Player").transform.Rotate(new Vector3(0, number, 0));
	}
	
	public void rotateArmUpDownBy(int number)
	{
		GameObject.FindWithTag("Player").transform.Rotate(new Vector3(number, 0, 0));
	}
	
	//trigger control functions for voice or mind
	public void rotateArmToLeft()
	{
		Log("rotate left");
//		OpenEars.Speak ("Rotating left.");
		
		turnOfAllBooleans();
		isGoingLeft = true;
		
		var www = new WWW("http://apps.vitapoly.com/apps/mcrobot/post.php?cmd=left");

		
//		var greenArrow = GameObject.FindWithTag("greenArrow");
//		if(greenArrow.transform.localPosition.y  == -65)
//		{
//			//do nothing
//		}
//		if(greenArrow.transform.localPosition.y  == -80)
//		{
//			greenArrow.transform.Translate(0, 15, 0); //moves up
//		}
//		if(greenArrow.transform.localPosition.y  == -95)
//		{
//			greenArrow.transform.Translate(0, 30, 0); //moves up
//		}
	}
	
	public void rotateArmToRight()
	{
		Log("rotate right");
//		OpenEars.Speak ("Rotating right.");

		turnOfAllBooleans();
		isGoingRight = true;
		
		var www = new WWW("http://apps.vitapoly.com/apps/mcrobot/post.php?cmd=right");
		
//		var greenArrow = GameObject.FindWithTag("greenArrow");
//		
//		if(greenArrow.transform.localPosition.y == -65)
//		{
//			greenArrow.transform.Translate(0, -15, 0); //moves up
//		}
//		if(greenArrow.transform.localPosition.y == -80)
//		{
//			//do nothing
//		}
//		if(greenArrow.transform.localPosition.y  == -95)
//		{
//			greenArrow.transform.Translate(0, 15, 0); //moves up
//		}
//		
	}
	
	void openHand()
	{
		//Debug.Log("open hand");
		var clawLeft = GameObject.FindWithTag("clawLeft");
		var clawRight = GameObject.FindWithTag("clawRight");
		if (!isOpen) {
			OpenEars.Speak ("Opening claw.");
			clawLeft.transform.Rotate(new Vector3(0, -40, 0));
			clawRight.transform.Rotate(new Vector3(0, 40, 0));
			
			var www = new WWW("http://apps.vitapoly.com/apps/mcrobot/post.php?cmd=open");

			isOpen = true;
		}
	}
	
	
	void closeHand()
	{
		var clawLeft = GameObject.FindWithTag("clawLeft");
		var clawRight = GameObject.FindWithTag("clawRight");
		if (isOpen) {
			OpenEars.Speak ("OK, don't crush me.");
			clawLeft.transform.Rotate(new Vector3(0, 40, 0));
			clawRight.transform.Rotate(new Vector3(0, -40, 0));
			
			var www = new WWW("http://apps.vitapoly.com/apps/mcrobot/post.php?cmd=close");

			isOpen = false;
		}
	
	}
	
	/*
	void rotateArmToUp()
	{
		turnOfAllBooleans();
		isGoingUp = true;
	}
	
	void rotateArmToDown()
	{
		turnOfAllBooleans();
		isGoingDown = true;
	}*/
		
	void turnOfAllBooleans()
	{
		isGoingLeft = false;
		isGoingRight = false;
		isGoingUp = false;
		isGoingDown = false;
	}
	
	string _log = "Debug log:";
	Vector2 _scrollPosition = Vector2.zero;
	
	void OnGUILog() {
		GUILayout.BeginArea(new Rect(50, Screen.height - 200, Screen.width - 100, 200));
		_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
		GUI.skin.box.wordWrap = true;
		GUI.skin.box.alignment = TextAnchor.UpperLeft;
		GUI.skin.box.fontSize = 40;
		GUILayout.Box(_log, GUILayout.ExpandHeight(true));
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}
	
	public void Log(string str) {
		_log += "\n" + str;
		_scrollPosition.y = Mathf.Infinity;
//		Debug.Log(str);
	}
}
