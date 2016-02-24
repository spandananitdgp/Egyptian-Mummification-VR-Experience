using UnityEngine;
using System.Collections;

public class ExitGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Cardboard.SDK.BackButtonPressed) {
			Application.Quit();
		}
	}
}
