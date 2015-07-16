using UnityEngine;
using System.Collections;

public class Blades : MonoBehaviour {

	public float startTime;
	public SpriteRenderer sprite;
	private float velocity = 1f;
	public float startScale = 0f;
	public Color color;
	// Use this for initialization
	void Start () {
		startTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		float fade = Mathf.Lerp (1f, 0f, startTime);
		this.transform.localScale = new Vector3 (startScale + (1-fade), startScale + (1- fade), 1f);
		startTime += Time.deltaTime;
		sprite.color = new Color(color.r,color.g,color.b,fade);
		if (startTime > 1f) {
			Destroy(this.gameObject);
		}
	
	}
}