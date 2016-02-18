using UnityEngine;
using System.Collections;

public class ChangeBodyTransparency : MonoBehaviour {

	public GameObject thePlayer;
	public Texture transparentTexture;
	public Texture normalTexture;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float distanceBetweenPlayerAndBody = Vector3.Distance (thePlayer.transform.position, this.transform.position);
		if (distanceBetweenPlayerAndBody <= 5.0f) {
			Debug.Log("Change Transparency");
			Material material = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));
			material.mainTexture = transparentTexture;
			GetComponent<SkinnedMeshRenderer>().material = material;
		}
	}
}
