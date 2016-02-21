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
	public Texture mummyTexture;
	private bool isBodyMummified = false;
	private bool isBodyPickable = false;
	private bool isBodyPicked = false;
	private Material mummyTextureMaterial;
	public GameObject sarcophagus;
	private bool isSarcophagusLidOpen = false;
	private bool isBodyInCoffin = false;
	private bool isSarcophagusLidClosed = false;

	// Use this for initialization
	void Start () {
		mummyTextureMaterial = new Material (Shader.Find ("Mobile/Diffuse"));
		mummyTextureMaterial.mainTexture = mummyTexture;
		mummyTextureMaterial.mainTextureScale = new Vector2 (5.0f, 5.0f);
		GameObject bodyinHand = GameObject.Find ("humanbodyinHand/Mhx2sample:Body");
		bodyinHand.GetComponent<SkinnedMeshRenderer> ().material = mummyTextureMaterial;
		GameObject bodyinBasket = GameObject.Find ("humanbodyinBasket/Mhx2sample:Body");
		bodyinBasket.GetComponent<SkinnedMeshRenderer> ().material = mummyTextureMaterial;

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
			if (distanceBetweenPlayerAndBody <= 3.0f) {
				//Debug.Log("Change Transparency");
				Material material = new Material (Shader.Find ("Legacy Shaders/Transparent/Diffuse"));
				material.mainTexture = transparentTexture;
				theBody.GetComponent<SkinnedMeshRenderer> ().material = material;
				isBodyTransparent = true;
			}
		}

		if (isBodyMummified && !isBodyPickable) {
			BoxCollider bodyCollider = theBody.transform.parent.gameObject.AddComponent<BoxCollider>();
			bodyCollider.center = new Vector3(-0.09494729f, 5.081681f, 0.3331702f);
			bodyCollider.size = new Vector3(2.831509f, 9.839473f, 1.66662f);
			theBody.transform.parent.gameObject.tag = "Pickable";
			isBodyPickable = true;
		}

		if (Input.GetButtonDown("Fire1")) {
			if (isBodyTransparent && areInternalOrgansRemoved) {
				Material material = new Material (Shader.Find ("Mobile/Diffuse"));
				material.mainTexture = normalTexture;
				theBody.GetComponent<SkinnedMeshRenderer> ().material = material;
			}

			if(isSarcophagusLidOpen && isBodyInCoffin) {
				isSarcophagusLidClosed = sarcophagus.GetComponent<MoveSarcophagusLid>().closeSarcophagusLid();
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
					} else if(objectInHand.name.Contains("roll")) {
						mummify();
						dropObject(objectInHand);
					} else if(objectInHand.name.Contains("body")) {
						if(isSarcophagusLidOpen) {
							dropObject(objectInHand);
							Debug.Log("Now close the sarcophagus lid."); //TODO: Play Audio here.
						}
					} else {
						dropObject(objectInHand);
					}
				}
			}

			if (isBodyPicked) {
				float distanceBetweenPlayerAndSarcophagus = Vector3.Distance(thePlayer.transform.position, sarcophagus.transform.position);
				if(distanceBetweenPlayerAndSarcophagus <= 3.0f) {
					isSarcophagusLidOpen = sarcophagus.GetComponent<MoveSarcophagusLid>().openSarcophagusLid();
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
					if(pickableObject.name.Contains("body")) {
						isBodyPicked = true;
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
			Destroy (objectInHand);
			knifeInHand.SetActive (false);
		} else if(objectInHand.name.Contains ("roll")) {
			GameObject roll = pickableGameObjectsReference[objectInHand.name.Replace("inHand", "")];
			roll.SetActive(true);
			Destroy (objectInHand);
			knifeInHand.SetActive (false);
		} else {
			GameObject basketedObject = basketedGameObjectsReference[objectInHand.name.Replace("inHand", "inBasket")];
			basketedObject.SetActive (true);
			if(objectInHand.name.Contains("body")) {
				knifeInHand.SetActive (false);
				isBodyInCoffin = true;
			} else {
				knifeInHand.SetActive (true);
			}
			Destroy (objectInHand);
		}
		isKnifePicked = true;
		isObjectInHand = false;

		if (isBrainPicked && areLungsPicked && areIntestinesPicked) {
			areInternalOrgansRemoved = true;
			Debug.Log("Good! Now pour the salt from the pot on the body."); //TODO: Play Audio here.
		}
	}

	void pourSalt() {
		saltOnTable.SetActive (true);
		Debug.Log ("Now take the resin dipped linen strips and wrap the body."); //TODO: Play Audio here.
	}

	void mummify() {
		theBody.GetComponent<SkinnedMeshRenderer> ().material = mummyTextureMaterial;
		isBodyMummified = true;
		Debug.Log ("Now we put the mummy in the sarcophagus. Pick up the body and follow the directions."); //TODO: Play Audio here.
	}
}
