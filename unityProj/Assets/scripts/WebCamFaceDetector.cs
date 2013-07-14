using System;
using System.Collections;
using UnityEngine;
using U3DXT.Core;
using U3DXT.iOS.Native.Foundation;
using U3DXT.iOS.Native.UIKit;
using U3DXT.Utils;
using U3DXT.iOS.Native.Internals;
using System.IO;
using U3DXT.iOS.GUI;
using U3DXT.iOS.CoreImage;
using U3DXT.iOS.Native.CoreImage;
using U3DXT.iOS.Native.CoreGraphics;

public class WebCamFaceDetector : MonoBehaviour {
	
	FaceDetector faceDetector;
	WebCamTexture webCam;
	Face[] faces;
	
	void Start() {
		
		if (CoreXT.IsDevice)
			webCam = new WebCamTexture("Front Camera");
		else
			webCam = new WebCamTexture();
		
		RotatableGUIItem guiItem = ((RotatableGUIItem)(GameObject.Find ("CameraVideo").GetComponent("RotatableGUIItem")));
		guiItem.texture = webCam;
		guiItem.size = new Vector2(Screen.width / 8, Screen.height / 8);
		guiItem.relativePosition = new Vector2(-Screen.width / 16, Screen.height / 16);
		
		if (CoreXT.IsDevice) {
			
			faceDetector = new FaceDetector(false, false);
			faceDetector.IsMirrored = true;
		}

		StartDetect();
	}
	
	public void StartDetect() {
		webCam.Play();
	}
	
	public void StopDetect() {
		webCam.Stop();
	}
	
	void OnGUI() {
		
//		if (CoreXT.IsDevice) {
		{
			if (faces != null) {
				foreach (var face in faces) {
					GUI.Box(face.Bounds, "");
				}
			}
		}
	}
	
	void Update() {
		if (CoreXT.IsDevice) {
			if (webCam.didUpdateThisFrame) {
				CGImageOrientation orientation = CGImageOrientation.RotatedLeft;
				switch (webCam.videoRotationAngle) {
				case 0:
					orientation = CGImageOrientation.Default;
					break;
				case 90:
					orientation = CGImageOrientation.RotatedLeft;
					break;
				case 180:
					orientation = CGImageOrientation.UpsideDown;
					break;
				case 270:
					orientation = CGImageOrientation.RotatedRight;
					break;
				}
				
				var ciimage = new CIImage(CGImage.FromWebCamTexture(webCam));
				faceDetector.ProjectedScale = Screen.width / webCam.width;
				faces = faceDetector.DetectInImage(ciimage, orientation);
//				foreach (var face in faces) {
//					Log("face: " + face.Bounds + ", " + face.HasMouthPosition + ", " + face.LeftEyePosition + ", " + face.RightEyePosition);
//				}
				
				if (faces.Length == 1) {
					var face = faces[0];
					if (face.Bounds.center.x < (Screen.width / 2))
						GameObject.Find("Main Camera").GetComponent<Main>().rotateArmToLeft();
					else
						GameObject.Find("Main Camera").GetComponent<Main>().rotateArmToRight();
				}
			}
		}
	}
}
