using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using U3DXT.Core;
using System;

public class MindWave : MonoBehaviour {
	
	private static EventHandler<MindWaveSensedEventArgs> _sensedHandlers;
	public static event EventHandler<MindWaveSensedEventArgs> Sensed {
		add { _sensedHandlers += value; }
		remove { _sensedHandlers -= value; }
	}


	[DllImport ("__Internal")]
	protected static extern void UP_MindWave_init();
	
	[DllImport ("__Internal")]
	protected static extern void UP_MindWave_startListening();

	[DllImport ("__Internal")]
	protected static extern void UP_MindWave_stopListening();

	private static GameObject _gameObj = null;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static void Init() {
		if (CoreXT.IsDevice) {
			if (_gameObj != null)
				return;
			
			_gameObj = new GameObject("MindWaveGameObj");
			_gameObj.AddComponent("MindWave");

			UP_MindWave_init();
		}
	}
	
	public static void StartListening() {
		UP_MindWave_startListening();
	}
	
	public static void StopListening() {
		UP_MindWave_stopListening();
	}
		
	public void OnSensed(string cmd) {
		if (_sensedHandlers != null)
			_sensedHandlers(null, new MindWaveSensedEventArgs(cmd));
	}
}
