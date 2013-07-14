using UnityEngine;
 
[ExecuteInEditMode()] 
public class RotatableGUIItem : MonoBehaviour
{
	public Texture texture = null;
	public float angle = 0;
	public Vector2 size = new Vector2(128, 128);
	 
	//this will overwrite the items position
	public AlignmentScreenpoint ScreenpointToAlign = AlignmentScreenpoint.TopLeft;
	public Vector2 relativePosition = new Vector2(0, 0);
	 
	Vector2 pos = new Vector2(0, 0);
	 
	Rect rect;
	Vector2 pivot;
	 
	void Start() 
	{
	    UpdateSettings();
	}
	 
	void UpdateSettings()
	{
	    Vector2 cornerPos = new Vector2(0, 0);
	 
	    //overwrite the items position
	    switch (ScreenpointToAlign)
	    {
	       case AlignmentScreenpoint.TopLeft:
	         cornerPos =new Vector2(0, 0);
	         break;
	       case AlignmentScreenpoint.TopMiddle:
	         cornerPos =new Vector2(Screen.width/2, 0);
	         break;
	       case AlignmentScreenpoint.TopRight:
	         cornerPos = new Vector2(Screen.width, 0);
	         break;
	       case AlignmentScreenpoint.LeftMiddle:
	         cornerPos = new Vector2(0, Screen.height / 2);
	         break;
	       case AlignmentScreenpoint.RightMiddle:
	         cornerPos = new Vector2(Screen.width, Screen.height / 2);
	         break;
	       case AlignmentScreenpoint.BottomLeft:
	         cornerPos = new Vector2(0, Screen.height);
	         break;
	       case AlignmentScreenpoint.BottomMiddle:
	         cornerPos = new Vector2(Screen.width/2, Screen.height);
	         break;
	       case AlignmentScreenpoint.BottomRight:
	         cornerPos = new Vector2(Screen.width, Screen.height);
	         break;
	       default:
	         break;
	    }
	 
	    pos = cornerPos + relativePosition;
	    rect = new Rect(pos.x - size.x * 0.5f, pos.y - size.y * 0.5f, size.x, size.y);
	    pivot = new Vector2(rect.xMin + rect.width * 0.5f, rect.yMin + rect.height * 0.5f);
	}
	 
	void OnGUI()
	{
	    if (Application.isEditor)
	    {
	       UpdateSettings();
	    }
	    Matrix4x4 matrixBackup = GUI.matrix;
	    GUIUtility.RotateAroundPivot(angle, pivot);
		GUIUtility.ScaleAroundPivot(new Vector2(-1,1), new Vector2(0,0));
	    GUI.DrawTexture(rect, texture);
	    GUI.matrix = matrixBackup;
	}
	 
	public enum AlignmentScreenpoint
	{
	    TopLeft, TopMiddle, TopRight,
	    LeftMiddle, RightMiddle,
	    BottomLeft, BottomMiddle, BottomRight
	}
}

