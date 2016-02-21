using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
	public string sceneName;
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player") {
			SceneManager.LoadScene (sceneName);
		}
	}
}
