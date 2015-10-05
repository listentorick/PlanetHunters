using UnityEngine;
using System.Collections;

public class SelectedRenderer : MonoBehaviour {

	public Body body;
	public SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (body != null) {
			spriteRenderer.enabled = body.IsSelected;
		}
	}
}
