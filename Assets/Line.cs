using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour {


	public Body target1;
	public Body target2;
	private Vector3[] corners = new Vector3[4];
	private Vector2 initialSize;
	//private float initialHeight;
	private float width;
	private float height;
	private RectTransform rectTransform;
	// Use this for initialization
	void Start () {
		rectTransform = this.GetComponent<RectTransform> ();
		rectTransform.GetWorldCorners (corners);
		width = corners [2].x - corners [1].x;
		height = corners [1].y - corners [0].y;
		initialSize = rectTransform.sizeDelta;
	}


	float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n){
		// angle in [0,180]
		float angle = Vector3.Angle(a,b);
		float sign = Mathf.Sign(Vector3.Dot(n,Vector3.Cross(a,b)));
		
		// angle in [-179,180]
		float signed_angle = angle * sign;
		
		// angle in [0,360] (not used but included here for completeness)
		//float angle360 =  (signed_angle + 180) % 360;
		
		return signed_angle;
	}

	
	// Update is called once per frame
	void Update () {

		Vector3 diff = target2.gameObject.transform.position - target1.transform.position;
		Vector3 direction = diff.normalized;

		float length = diff.magnitude/2f;
		Vector3 position = direction * length;

		//float angle =  SignedAngleBetween (target1.transform.position, target2.transform.position, Vector3.right);
		//this.transform.rotation = Quaternion.AngleAxis(angle,Vector3.right);


		float angle = Mathf.Atan2 (diff.y, diff.x) * Mathf.Rad2Deg;
		//angle = (angle > 0 ? angle : (2 * Mathf.PI + angle)) * 360 / (2 * Mathf.PI);
		//angle += Mathf.Ceil( -angle / 360 ) * 360;
		//angle = (angle + 360) % 360;
		this.transform.rotation = Quaternion.AngleAxis(angle + 90,Vector3.forward);
	

		this.transform.position = target1.gameObject.transform.position + position + new Vector3 (0, 0, 2f);
		rectTransform.sizeDelta = new Vector2 (initialSize.x, 2 * initialSize.y  * (length/height));


	
	}
}
