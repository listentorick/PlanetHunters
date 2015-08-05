using UnityEngine;
using System.Collections;

public class TractorBeamRenderer : MonoBehaviour {

	public SpriteRenderer sprite;
	public PolygonCollider2D target;
	private float initialLength;

	// Use this for initialization
	void Start () {
		initialLength = sprite.bounds.size.x;
		sprite.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			sprite.enabled = true;
			//we center the sprite between the two objects
			Vector3 diff = target.gameObject.transform.position - this.transform.parent.transform.position;
			Vector3 direction = diff.normalized;
			Vector3 position = direction * (diff.magnitude / 2f);
			sprite.transform.localPosition = position;
			//sprite.transform.LookAt(target.gameObject.transform.position);

			Quaternion rotation = Quaternion.LookRotation
				(diff, transform.parent.transform.TransformDirection (Vector3.up));

			rotation = rotation * Quaternion.Euler (0, 0, 90);

		
			sprite.transform.rotation = new Quaternion (0, 0, rotation.z, rotation.w);

			float multiplier = initialLength / diff.magnitude;
			sprite.transform.localScale = new Vector3 (1, multiplier, multiplier);


		} else {
			sprite.enabled = false;
		}
	
	}
}
