using UnityEngine;
using System.Collections;

public class ShipIndicator : MonoBehaviour {

	public Ship ship;
	Vector3 bottomLeft;
	Vector3 bottomRight;
	Vector3 topRight;

	public SpriteRenderer spriteRenderer;
	public Sprite topIndicator;
	public Sprite leftIndicator;
	public Sprite bottomIndicator;
	public Sprite rightIndicator;

	// Use this for initialization
	void Start () {
		Camera cam = Camera.main;
		bottomLeft = cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane));  
		bottomRight = cam.ViewportToWorldPoint(new Vector3(1,0,cam.nearClipPlane));
		topRight = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane));
		
		//float width = (topright - topleft).magnitude;
		//float height = (bottomRight - topright).magnitude;

	}

	//Vector2 GetOffset(Sprite sprite){
	
	//}
	
	// Update is called once per frame
	void Update () {
	
		//if (IsShipOffScreen ()) {
		if (!ship.gameObject.activeSelf) {
			this.transform.position = new Vector3(100,100,this.transform.position.z);
			return;
		}

			//where is the ship?
		if (ship.gameObject.transform.position.y > topRight.y) {

			spriteRenderer.sprite = topIndicator;
	
			//position this thing on the top
			this.transform.position = new Vector3 (ship.gameObject.transform.position.x, topRight.y - spriteRenderer.sprite.bounds.extents.y, this.transform.position.z);
			
		} else if (ship.gameObject.transform.position.y < bottomRight.y) {
			spriteRenderer.sprite = bottomIndicator;
			//position this thing on the top
			this.transform.position = new Vector3 (ship.gameObject.transform.position.x, bottomRight.y + spriteRenderer.sprite.bounds.extents.y, this.transform.position.z);
			
		} else if (ship.gameObject.transform.position.x < bottomLeft.x) {
			spriteRenderer.sprite = leftIndicator;
			//position this thing on the top
			this.transform.position = new Vector3 (bottomLeft.x + spriteRenderer.sprite.bounds.extents.x, ship.gameObject.transform.position.y, this.transform.position.z);
			
		} else if (ship.gameObject.transform.position.x > bottomRight.x) {
			spriteRenderer.sprite = rightIndicator;
			//position this thing on the top
			this.transform.position = new Vector3 (bottomRight.x- spriteRenderer.sprite.bounds.extents.x, ship.gameObject.transform.position.y, this.transform.position.z);
			
		} else {
			this.transform.position = new Vector3(100,100,this.transform.position.z);
		}
		
		
		
		//}
	}

	bool IsShipOffScreen() {
		return true;
	}
}
