using UnityEngine;
using System.Collections;

public class Body : MonoBehaviour {

	public ParticleSystem thruster;

	public Vector2 lastPosition;
	public Vector2 position;
	public Vector2 velocity;
	public Vector2 acceleration;
	public float mass;
	public bool canMove;
	public bool inOrbit;
	public bool justEnteredOrbit;
	public Body parentBody;
	public Vector2 additionalForce = new Vector2(0,0);
	public float soi;
	public bool IsSelected;
	//protected float scale = 100000f;
	private bool isWrappingX;
	private bool isWrappingY;
	public bool canAlign;
	//public bool canAlignToVelocity;

	private Transform rendererTransform;

	public Transform GetRendererTransform() {
		if (rendererTransform == null) {
			rendererTransform = this.gameObject.transform.GetChild (0);
		}
		return rendererTransform;
	}

	public void AlignToVector(Vector2 v) {

		if (!canAlign)
			return;
		
		var angle = Mathf.Atan2 (v.y, v.x) * Mathf.Rad2Deg;
		
		if (v.x < 0 && v.y > 0) {
			//topleft
			angle -= 90;
		}
		if (v.x > 0 && v.y > 0) {
			angle += 270;
		}
		if (v.x > 0 && v.y < 0) {
			angle -= 90;
		}
		if (v.x < 0 && v.y < 0) {
			angle += 270;
		}
		if (v.x == 0 && v.y < 0) {
			angle = 180;
		}
		if (v.x == 0 && v.y > 0) {
			angle = 0;
		}
		
		if (v.x < 0 && v.y == 0) {
			angle = 90;
		}
		if (v.x > 0 && v.y == 0) {
			angle = -90;
		}
		
		if (rendererTransform == null) {
			rendererTransform = this.gameObject.transform.GetChild (0);
		}
		
		rendererTransform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		
	}

	public  bool IsRendererVisible() {
		if (rendererTransform == null)
			return false;
		return rendererTransform.gameObject.GetComponent<SpriteRenderer>().isVisible;
	}


	public void Update () {

		this.transform.position = new Vector3(position.x/GameController.SCALE, position.y/GameController.SCALE, this.transform.position.z);
	

	}






}
