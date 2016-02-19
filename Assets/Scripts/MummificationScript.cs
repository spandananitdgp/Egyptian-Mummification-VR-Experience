using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MummificationScript : MonoBehaviour {

	public Camera mainCamera;
	private Dictionary<string, GameObject> gameObjectsInHandReference;
	private Dictionary<string, GameObject> pickableGameObjectsReference;
	private bool isKnifePicked = false;
	private Transform knifeInHandOriginalTransform;
	private GameObject knifeInHand;
	private bool isObjectInHand = false;
	private bool isBrainPicked = false;

	// Use this for initialization
	void Start () {
		GameObject[] gameObjectsInHand = GameObject.FindGameObjectsWithTag ("Picked");
		gameObjectsInHandReference = new Dictionary<string, GameObject> ();
		foreach (GameObject gameObject in gameObjectsInHand) {
			gameObjectsInHandReference.Add(gameObject.name, gameObject);
			gameObject.SetActive(false);
		}

		GameObject[] pickableGameObjects = GameObject.FindGameObjectsWithTag ("Pickable");
		pickableGameObjectsReference = new Dictionary<string, GameObject> ();
		foreach (GameObject gameObject in pickableGameObjects) {
			pickableGameObjectsReference.Add(gameObject.name, gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			GameObject objectInHand = null;
			if (isKnifePicked) {
				knifeInHand = gameObjectsInHandReference["KnifeV1inHand"];
				knifeInHandOriginalTransform = knifeInHand.transform;
				Debug.Log("Please remove the brain."); //TODO: Play Audio here.
			}

			if(!isObjectInHand) {
				objectInHand = pickObject();
			} else {
				if(objectInHand != null) {
					Debug.Log("Dropping Object");
					dropObject(objectInHand);
				}
			}
		}
	}

	GameObject pickObject() {
		//Debug.Log ("isKnifePicked :: " + isKnifePicked);
		RaycastHit hitInfo;
		GameObject objectInHand = null;
		//Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.rotation * Vector3.forward);
		if(Physics.Raycast(new Ray(mainCamera.transform.position, mainCamera.transform.rotation * Vector3.forward), out hitInfo)) {
			//Debug.Log("Hit something " + hitInfo.transform.name);
			if(hitInfo.transform.gameObject.tag == "Pickable") {
				//Debug.Log("Object Picked");
				GameObject pickableObject = hitInfo.transform.gameObject;
				if(isKnifePicked) {
					knifeInHand.SetActive(false);
					if(pickableObject.name.Contains("Brain")) {
						isBrainPicked = true;
					}
					isObjectInHand = true;
				} else {
					if(pickableObject.name.Contains("Knife")) {
						isKnifePicked = true;
					} else {
						Debug.Log("Please pick up the knife first!"); //TODO: Play Audio here.
					}
				}

				pickableObject.SetActive(false);
				GameObject objectPickedInHand = gameObjectsInHandReference[pickableObject.name + "inHand"];
				Debug.Log("Object in Hand :: " + objectPickedInHand.name);
				objectPickedInHand.SetActive(true);
				objectInHand = objectPickedInHand;
			}
		}

		return objectInHand;
	}

	void dropObject(GameObject objectInHand) {
		RaycastHit hitInfo;
		if (Physics.Raycast (new Ray (mainCamera.transform.position, mainCamera.transform.rotation * Vector3.forward), out hitInfo)) {
			Debug.Log("Hit Object :: " + hitInfo.transform.name);
			if(hitInfo.transform.gameObject.tag == "Dropbasket") {
				objectInHand.transform.position = hitInfo.point;
			}
		}
	}
}
