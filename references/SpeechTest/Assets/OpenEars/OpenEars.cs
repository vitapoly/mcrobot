using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using U3DXT.Core;
using System;

public class OpenEars : MonoBehaviour {
	
	private static EventHandler<OpenEarsHeardEventArgs> _heardHandlers;
	public static event EventHandler<OpenEarsHeardEventArgs> Heard {
		add { _heardHandlers += value; }
		remove { _heardHandlers -= value; }
	}


	[DllImport ("__Internal")]
	protected static extern void UP_OpenEars_init(string csWords);
	
	[DllImport ("__Internal")]
	protected static extern void UP_OpenEars_startListening();

	[DllImport ("__Internal")]
	protected static extern void UP_OpenEars_stopListening();

	[DllImport ("__Internal")]
	protected static extern void UP_OpenEars_speak(string phrase);

	private static GameObject _gameObj = null;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static void Init(string[] words) {
		if (CoreXT.IsDevice) {
			if (_gameObj != null)
				return;
			
			_gameObj = new GameObject("OpenEarsGameObject");
			_gameObj.AddComponent("OpenEars");

			UP_OpenEars_init(string.Join(",", words));
		}
	}
	
	public static void StartListening() {
		UP_OpenEars_startListening();
	}
	
	public static void StopListening() {
		UP_OpenEars_stopListening();
	}
	
	public static void Speak(string phrase) {
		UP_OpenEars_speak(phrase);
	}
	
	public void OnHeard(string phrase) {
		if (_heardHandlers != null)
			_heardHandlers(null, new OpenEarsHeardEventArgs(phrase));
	}
}
