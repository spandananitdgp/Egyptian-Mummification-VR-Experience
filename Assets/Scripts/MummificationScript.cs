using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MummificationScript : MonoBehaviour {

	public Camera mainCamera;
	private Dictionary<string, GameObject> gameObjectsInHandReference;
	private Dictionary<string, GameObject> pickableGameObjectsReference;
	private Dictionary<string, GameObject> basketedGameObjectsReference;
	private bool isKnifePicked = false;
	private Transform knifeInHandOriginalTransform;
	private GameObject knifeInHand;
	private bool isObjectInHand = false;
	private bool isBrainPicked = false;
	private bool areLungsPicked = false;
	private bool areIntestinesPicked = false;
	private GameObject objectInHand;
	private bool areInternalOrgansRemoved = false;
	private bool isPotPicked = false;
	private GameObject saltOnTable;
	public GameObject thePlayer;
	public GameObject theBody;
	public Texture transparentTexture;
	public Texture normalTexture;
	private bool isBodyTransparent = false;

	// Use this for initialization
	void Start () {
		objectInHand = null;
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

		GameObject[] basketedGameObjects = GameObject.FindGameObjectsWithTag ("Basketed");
		basketedGameObjectsReference = new Dictionary<string, GameObject> ();
		foreach (GameObject gameObject in basketedGameObjects) {
			basketedGameObjectsReference.Add(gameObject.name, gameObject);
			gameObject.SetActive(false);
		}

		saltOnTable = GameObject.Find ("SaltOnTable");
		saltOnTable.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!isBodyTransparent) {
			float distanceBetweenPlayerAndBody = Vector3.Distance (thePlayer.transform.position, theBody.transform.position);
			if (distanceBetweenPlayerAndBody <= 5.0f) {
				//Debug.Log("Change Transparency");
				Material material = new Material (Shader.Find ("Legacy Shaders/Transparent/Diffuse"));
				material.mainTexture = transparentTexture;
				theBody.GetComponent<SkinnedMeshRenderer> ().material = material;
				isBodyTransparent = true;
			}
		}

		if (Input.GetButtonDown("Fire1")) {
			if (isBodyTransparent && areInternalOrgansRemoved) {
				Material material = new Material (Shader.Find ("Mobile/Diffuse"));
				material.mainTexture = normalTexture;
				theBody.GetComponent<SkinnedMeshRenderer> ().material = material;
			}

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
					if(objectInHand.name.Contains("Pot")) {
						pourSalt();
						dropObject(objectInHand);
					} else {
						dropObject(objectInHand);
					}
				}
			}
		}
	}

	GameObject pickObject() {
		//Debug.Log ("isKnifePicked :: " + isKnifePicked);
		RaycastHit hitInfo;
		GameObject objectInHand = null;
		//Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.rotation * Vector3.forward);
		if(Physics.Raycast(Cardboard.SDK.GetComponentInChildren<CardboardHead>().Gaze, out hitInfo)) {
			//Debug.Log("Hit something " + hitInfo.transform.name);
			if(hitInfo.transform.gameObject.tag == "Pickable") {
				//Debug.Log("Object Picked");
				GameObject pickableObject = hitInfo.transform.gameObject;
				if(isKnifePicked) {
					knifeInHand.SetActive(false);
					if(pickableObject.name.Contains("Brain")) {
						isBrainPicked = true;
					}
					if(pickableObject.name.Contains("lung")) {
						areLungsPicked = true;
					}
					if(pickableObject.name.Contains("Intestine")) {
						areIntestinesPicked = true;
					}
					isObjectInHand = true;
					isKnifePicked = false;
				} else {
					if(pickableObject.name.Contains("Knife")) {
						isKnifePicked = true;
					} else if(pickableObject.name.Contains("Pot")) {
						isPotPicked = true;
					}
					else {
						Debug.Log("Please pick up the knife first!"); //TODO: Play Audio here.
					}
				}

				pickableObject.SetActive(false);
				GameObject objectPickedInHand = gameObjectsInHandReference[pickableObject.name + "inHand"];
				Debug.Log("Object in Hand :: " + objectPickedInHand.name);
				objectPickedInHand.SetActive(true);
				objectInHand = objectPickedInHand;
			} else if(hitInfo.transform.gameObject.tag == "Unpickable") {
				if(hitInfo.transform.name == "Heart") {
					Debug.Log ("No! We must keep the heart with the body."); //TODO: Play Audio here.
				}
			}
		}

		return objectInHand;
	}

	void dropObject(GameObject objectInHand) {
		if (objectInHand.name.Contains ("Pot")) {
			GameObject saltPot = pickableGameObjectsReference[objectInHand.name.Replace("inHand", "")];
			saltPot.SetActive(true);
		} else {
			GameObject basketedObject = basketedGameObjectsReference[objectInHand.name.Replace("inHand", "inBasket")];
			basketedObject.SetActive (true);
		}
		Destroy (objectInHand);
		knifeInHand.SetActive (true);
		isKnifePicked = true;
		isObjectInHand = false;

		if (isBrainPicked && areLungsPicked && areIntestinesPicked) {
			areInternalOrgansRemoved = true;
			Debug.Log("Good! Now pour the salt from the pot on the body."); //TODO: Play Audio here.
		}
	}

	void pourSalt() {
		saltOnTable.SetActive (true);
	}
}
