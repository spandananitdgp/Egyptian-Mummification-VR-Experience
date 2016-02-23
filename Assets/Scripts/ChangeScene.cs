﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
	public string sceneName;
	void OnControllerColliderHit(ControllerColliderHit other)
	{
		Debug.Log ("Collided");
		if (other.gameObject.tag == "LoadScene") {
			SceneManager.LoadScene (sceneName);
		}
	}
}
