using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	const float leftRotationLimit  = -.5f;
	const float rightRotationLimit = .5f;
	const float upRotationLimit    = -.3f;
	const float downRotationLimit = .3f;
	
	bool isGoingLeft = false;
	bool isGoingRight = false;
	bool isGoingUp = false;
	bool isGoingDown = false;
	
	
	// Use this for initialization
	void Start () 
	{
		//openHand();
		//rotateArmToLeft();
		//rotateArmToRight();
		//rotateArmToUp();
		//rotateArmToDown();
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
				rotateArmLeftRightBy(-1);
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
				rotateArmLeftRightBy(1);
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
	
	void rotateArmLeftRightBy(int number)
	{
		GameObject.FindWithTag("Player").transform.Rotate(new Vector3(0, number, 0));
	}
	
	void rotateArmUpDownBy(int number)
	{
		GameObject.FindWithTag("Player").transform.Rotate(new Vector3(number, 0, 0));
	}
	
	//trigger control functions for voice or mind
	void rotateArmToLeft()
	{
		turnOfAllBooleans();
		isGoingLeft = true;
		
		var greenArrow = GameObject.FindWithTag("greenArrow");
		if(greenArrow.transform.localPosition.y  == -65)
		{
			//do nothing
		}
		if(greenArrow.transform.localPosition.y  == -80)
		{
			greenArrow.transform.Translate(0, 15, 0); //moves up
		}
		if(greenArrow.transform.localPosition.y  == -95)
		{
			greenArrow.transform.Translate(0, 30, 0); //moves up
		}
	}
	
	void rotateArmToRight()
	{
		turnOfAllBooleans();
		isGoingRight = true;
		var greenArrow = GameObject.FindWithTag("greenArrow");
		
		if(greenArrow.transform.localPosition.y == -65)
		{
			greenArrow.transform.Translate(0, -15, 0); //moves up
		}
		if(greenArrow.transform.localPosition.y == -80)
		{
			//do nothing
		}
		if(greenArrow.transform.localPosition.y  == -95)
		{
			greenArrow.transform.Translate(0, 15, 0); //moves up
		}
		
	}
	
	void openHand()
	{
		//Debug.Log("open hand");
		GameObject.FindWithTag("clawLeft").transform.Rotate(new Vector3(0, -40, 0));
		GameObject.FindWithTag("clawRight").transform.Rotate(new Vector3(0, 40, 0));
	}
	
	
	void closeHand()
	{
		GameObject.FindWithTag("clawLeft").transform.Rotate(new Vector3(0, 40, 0));
		GameObject.FindWithTag("clawRight").transform.Rotate(new Vector3(0, -40, 0));
	
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
	
	
}
