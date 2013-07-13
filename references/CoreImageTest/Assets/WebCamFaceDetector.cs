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
		
//		RotatableGUIItem guiItem = ((RotatableGUIItem)(GameObject.Find ("CameraVideo").GetComponent("RotatableGUIItem")));
//		guiItem.texture = webCam;
		guiTexture.texture = webCam;
		webCam.Play();
		guiTexture.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
//		guiTexture.gameObject.transform.Rotate(new Vector3(0, -90, 0));
//		guiTexture.gameObject.transform.localScale = new Vector3(-1, 0, 0);
		
		if (CoreXT.IsDevice) {
			
			SubscribeEvents();
			
			faceDetector = new FaceDetector(false, false);
			faceDetector.IsMirrored = true;
//			faceDetector.ProjectedScale = Screen.width / webCam.width;
			Log("Face Detector initialized.");
		} else {
			Log("Not on device.");
		}
	}
	
	void OnGUI() {
		
		if (CoreXT.IsDevice) {
			
			GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, 100));//Screen.height/2 - 50));
			GUILayout.BeginHorizontal();

//			if (GUILayout.Button("Camera", GUILayout.ExpandHeight(true))) {
//				GUIXT.ShowImagePicker(UIImagePickerControllerSourceType.Camera);
//			}
//	
//			if (GUILayout.Button("Photo Library", GUILayout.ExpandHeight(true))) {
//				GUIXT.ShowImagePicker(UIImagePickerControllerSourceType.PhotoLibrary);
//			}
		
			if (GUILayout.Button("Front/Back", GUILayout.ExpandHeight(true))) {
				webCam.Stop();
				webCam.deviceName = (webCam.deviceName == "Front Camera") ? "Back Camera" : "Front Camera";
				webCam.Play();
				guiTexture.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
			}

			if (GUILayout.Button("Clear Log", GUILayout.ExpandHeight(true))) {
				_log = "";
			}
	
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
			
			if (faces != null) {
				foreach (var face in faces) {
					GUI.Box(face.Bounds, "");
				}
			}
		}
		
		OnGUILog();
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
				foreach (var face in faces) {
					Log("face: " + face.Bounds + ", " + face.HasMouthPosition + ", " + face.LeftEyePosition + ", " + face.RightEyePosition);
				}
			}
		}
	}
	
	void SubscribeEvents() {
//		GUIXT.MediaPicked += delegate(object sender, MediaPickedEventArgs e) {
//			Log("MediaPicked");
//			var image = e.Image.ToTexture2D();
//			guiTexture.texture = image;
//			guiTexture.pixelInset = new Rect(0, 0, image.width, image.height);
//			faces = faceDetector.DetectInImage(image, e.Image.ImageOrientation);
//			foreach (var face in faces) {
//				Log("face: " + face.Bounds + ", " + face.HasMouthPosition + ", " + face.LeftEyePosition + ", " + face.RightEyePosition);
//			}
//		};
	}

	string _log = "Debug log:";
	Vector2 _scrollPosition = Vector2.zero;
	
	void OnGUILog() {
		GUILayout.BeginArea(new Rect(50, Screen.height / 2, Screen.width - 100, Screen.height / 2 - 50));
		_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
		GUI.skin.box.wordWrap = true;
		GUI.skin.box.alignment = TextAnchor.UpperLeft;
		GUILayout.Box(_log, GUILayout.ExpandHeight(true));
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}
	
	void Log(string str) {
		_log += "\n" + str;
		_scrollPosition.y = Mathf.Infinity;
	}
}
