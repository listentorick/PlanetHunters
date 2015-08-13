using UnityEngine;
using System.Collections;

public class Lock : MonoBehaviour {

	//public Body b;
	// Use this for initialization
	void Start () {
		SpriteRenderer r = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
		r.material.color = Color.grey; // Set to opaque black


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
