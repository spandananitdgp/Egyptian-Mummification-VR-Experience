using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CardboardAudioSource))]
public class PortalOpen : MonoBehaviour {
	public AudioClip portalOpen;
	public AudioClip portalSoundLoop;
	public bool autoOpen = false;
	public float portalPositionX;
	public float portalPositionY;
	public float portalPositionZ;
	public ParticleSystem portalEffect;
	private float waitBeforeOpen;
	private Vector3 portalOriginalPosition;
	private CardboardAudioSource portalSoundSource;
	private bool isOpenPortal = false;
	private bool isPortalSoundLoopPlaying = false;
	public static bool isExperienceComplete = false;

	// Use this for initialization
	void Start () {
		waitBeforeOpen = 2.0f;
		portalOriginalPosition = this.transform.localPosition;
		portalSoundSource = this.gameObject.GetComponent<CardboardAudioSource> ();
		portalSoundSource.clip = portalOpen;
		portalSoundSource.loop = false;
		if (isExperienceComplete) {
			autoOpen = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (autoOpen) {
			waitBeforeOpen -= Time.deltaTime;
			if (waitBeforeOpen <= 0.0f) {
				openPortal ();
			}
		}

		if (isOpenPortal) {
			this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, 
				new Vector3 (portalPositionX, portalPositionY, portalPositionZ), Time.deltaTime * 1.5f);
			if (!portalSoundSource.isPlaying && !isPortalSoundLoopPlaying) {
				portalSoundSource.clip = portalSoundLoop;
				portalSoundSource.loop = true;
				portalSoundSource.Play ();
				isPortalSoundLoopPlaying = true;
			}
		}
	}

	public void openPortal() {
		autoOpen = false;
		isOpenPortal = true;
		portalEffect.Play ();
		portalSoundSource.PlayDelayed (2.7f);
	}
}
