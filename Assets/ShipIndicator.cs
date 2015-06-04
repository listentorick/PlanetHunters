using UnityEngine;
using System.Collections;

public class ShipIndicator : MonoBehaviour {

	public Ship ship;
	Vector3 bottomLeft;
	Vector3 bottomRight;
	Vector3 topRight;

	public SpriteRenderer spriteRenderer;
	public Sprite horizontalIndicator;
	public Sprite verticalIndicator;

	// Use this for initialization
	void Start () {
		Camera cam = Camera.main;
		bottomLeft = cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane));  
		bottomRight = cam.ViewportToWorldPoint(new Vector3(1,0,cam.nearClipPlane));
		topRight = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane));
		
		//float width = (topright - topleft).magnitude;
		//float height = (bottomRight - topright).magnitude;

	}
	
	// Update is called once per frame
	void Update () {
	
		//if (IsShipOffScreen ()) {
		
			//where is the ship?
		if (ship.gameObject.transform.position.y > topRight.y) {

			spriteRenderer.sprite = horizontalIndicator;
			//position this thing on the top
			this.transform.position = new Vector3 (ship.gameObject.transform.position.x, topRight.y, this.transform.position.z);
			
		} else if (ship.gameObject.transform.position.y < bottomRight.y) {
			spriteRenderer.sprite = horizontalIndicator;
			//position this thing on the top
			this.transform.position = new Vector3 (ship.gameObject.transform.position.x, bottomRight.y, this.transform.position.z);
			
		} else if (ship.gameObject.transform.position.x < bottomLeft.x) {
			spriteRenderer.sprite = verticalIndicator;
			//position this thing on the top
			this.transform.position = new Vector3 (bottomLeft.x, ship.gameObject.transform.position.y, this.transform.position.z);
			
		} else if (ship.gameObject.transform.position.x > bottomRight.x) {
			spriteRenderer.sprite = verticalIndicator;
			//position this thing on the top
			this.transform.position = new Vector3 (bottomRight.x, ship.gameObject.transform.position.y, this.transform.position.z);
			
		} else {
			this.transform.position = new Vector3(100,100,this.transform.position.z);
		}
		
		
		
		//}
	}

	bool IsShipOffScreen() {
		return true;
	}
}
