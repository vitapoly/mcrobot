using UnityEngine;
using System.Collections;

public class ButtonMode : MonoBehaviour {
	/*
	Vector3 loc0 = new Vector3(-12.93f, -17f, 3.85f);
	Vector3 loc1 = new Vector3(-12.93f, -30f, 3f);
	Vector3 loc2 = new Vector3(-12.93f, -44f, 2.28f);
	*/
	int modeNum =0;
	
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (Input.GetMouseButtonUp(0))
		{
			var arrow  = GameObject.FindWithTag("modeArrow");
			
			modeNum ++;
			arrow.transform.Translate(new Vector3 (0,-15,0));
			
			if(modeNum == 3)
			{
				modeNum = 0;
				arrow.transform.Translate(new Vector3 (0,45,0));
				
				setMode(modeNum);
			}		
			
		}
	}
	
	void setMode(int num)
	{
		switch(num)
		{
			case 0: // voice
			
				break;
			case 1: // face
			
				break;
			case 2: //mind
			
				break;
		}
	}
	
	
}
