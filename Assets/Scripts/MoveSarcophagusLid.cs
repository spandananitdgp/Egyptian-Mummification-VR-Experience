using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CardboardAudioSource))]
public class MoveSarcophagusLid : MonoBehaviour {
	public CardboardAudioSource lidMoveSound;
	private bool openLid = false;
	private bool closeLid = false;

	// Use this for initialization
	void Start () {
		lidMoveSound = gameObject.GetComponent<CardboardAudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (openLid) {
			this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, 
			                                            new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 1.069f),
			                                                   Time.deltaTime * 1.4f);
		}

		if (closeLid) {
			this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, 
			                                            new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0.118f),
			                                            Time.deltaTime * 1.4f);
		}
	}

	public bool openSarcophagusLid() {
		lidMoveSound.Play ();
		openLid = true;
		return openLid;
	}

	public bool closeSarcophagusLid() {
		lidMoveSound.Play ();
		openLid = false;
		closeLid = true;
		return closeLid;
	}
}
