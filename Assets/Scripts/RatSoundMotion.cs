using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CardboardAudioSource))]
public class RatSoundMotion : MonoBehaviour {
	public GameObject thePlayer;
	private CardboardAudioSource soundSource;
	private bool isMoving = false;

	// Use this for initialization
	void Start () {
		soundSource = gameObject.GetComponent<CardboardAudioSource> ();
		soundSource.volume = 1.0f;
		soundSource.rolloffMode = AudioRolloffMode.Linear;
		soundSource.minDistance = 1.0f;
		soundSource.maxDistance = 20.0f;
	}
	
	// Update is called once per frame
	void Update () {
		float distanceBetweenPlayerAndRat = Vector3.Distance (thePlayer.transform.position, this.transform.position);
		if (distanceBetweenPlayerAndRat <= 5.0f) {
			soundSource.Play();
			isMoving = true;
		}
		Debug.Log("isMoving for " + this.gameObject.name + " = " + isMoving);
		if (isMoving) {
			transform.Translate (Vector3.forward * Time.deltaTime * 10.0f);
		}
		if (distanceBetweenPlayerAndRat >= 100.0f) {
			Destroy(gameObject);
		}
	}
}
