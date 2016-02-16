using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CardboardAudioSource))]
public class SandAudio : MonoBehaviour {
	private ParticleSystem sandSystem;
	private CardboardAudioSource sandFallingAudioSource;

	// Use this for initialization
	void Start () {
		sandSystem = GetComponent<ParticleSystem> ();
		sandFallingAudioSource = GetComponent<CardboardAudioSource> ();
		sandFallingAudioSource.volume = 0.06f;
		sandFallingAudioSource.pitch = 0.5f;
		sandFallingAudioSource.rolloffMode = AudioRolloffMode.Linear;
		sandFallingAudioSource.minDistance = 0;
		sandFallingAudioSource.maxDistance = 3;
	}
	
	// Update is called once per frame
	void Update () {
		float time = sandSystem.time;
		if(Mathf.Round(time) % 4 == 0) {
			sandFallingAudioSource.Play();
		}
	}
}
