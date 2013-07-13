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

public class OpenEarsTest : MonoBehaviour {
	
	void Start() {
		
		OpenEars.Heard += delegate(object sender, OpenEarsHeardEventArgs e) {
			Log("Heard: " + e.Phrase);
		};
	}
	
	void OnGUI() {
		
		GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, 100));
		GUILayout.BeginHorizontal();
	
		if (GUILayout.Button("Init", GUILayout.ExpandHeight(true))) {
			OpenEars.Init(new string[] {"LEFT", "RIGHT", "UP", "DOWN", "GRAB", "DROP"});
			Log("OpenEars initialized.");
		}
		
		if (GUILayout.Button("Start", GUILayout.ExpandHeight(true))) {
			OpenEars.StartListening();
			Log("Started listening.");
		}

		if (GUILayout.Button("Stop", GUILayout.ExpandHeight(true))) {
			OpenEars.StopListening();
			Log("Stopped listening.");
		}
		
		if (GUILayout.Button("Speak", GUILayout.ExpandHeight(true))) {
			var phrase = "Hello";
			OpenEars.Speak(phrase);
			Log("Saying " + phrase);
		}
		
		if (GUILayout.Button("Clear Log", GUILayout.ExpandHeight(true))) {
			_log = "";
		}

		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	
		OnGUILog();
	}
	
	void Update() {
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
