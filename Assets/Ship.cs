using UnityEngine;
using System.Collections;

public enum Cargo {

	Food, Medical, Technology
}

public class Ship : Body {

	public int cargo;
	public int maxCargo;
	public Cargo cargoType;
	private float rechargeTime = 0;
	private Transform shipRendererTransform;
	private bool isWrappingX;
	private bool isWrappingY;
	//private Renderer[] renderers;

	public float fuel = 1f;
	public float burnRate = 0.0025f;

	public void Start(){
		shipRendererTransform = this.gameObject.transform.GetChild (0);
		//renderers = this.GetComponentsInChildren<Renderer> ();
	}

	//public 


	public void Thrust(Vector2 thrust){
		if (fuel <= 0)
			return;
		additionalForce = thrust;
	}

	public void AlignToVector(Vector2 v) {

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

		if (shipRendererTransform == null) {
			shipRendererTransform = this.gameObject.transform.GetChild (0);
		}
		
		shipRendererTransform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
	
	}

	public void Update() {
	
		if (additionalForce != Vector2.zero && canMove) {
			fuel -= burnRate;
			thruster.Play();
			if(fuel<0){
				fuel = 0;
				additionalForce = Vector2.zero;
				rechargeTime =0;
			} else {
				AlignToVector(additionalForce);
			}
		} else {
			
			if(thruster!=null)thruster.Stop();
			if(rechargeTime>5f){ 
				fuel +=  Time.deltaTime/10;
				if(fuel>1){
					fuel = 1;
				}
			}
			
		}

		base.Update ();
		

		ScreenWrap ();
		rechargeTime += Time.deltaTime;
	
	
	}

	private bool IsRendererVisible() {
		return shipRendererTransform.gameObject.GetComponent<SpriteRenderer>().isVisible;
		//foreach (Renderer r in renderers) {
	//		if(r.isVisible==true){
	//			return true;
	//		}
	//	}
	//	return false;
	}

	void ScreenWrap()
	{

		if(IsRendererVisible())
		{
			isWrappingX = false;
			isWrappingY = false;
			return;
		}
		
		if(isWrappingX && isWrappingY) {
			return;
		}
		
		var cam = Camera.main;
		var viewportPosition = cam.WorldToViewportPoint(transform.position);
		var newPosition = transform.position;

		if (!isWrappingX && ((viewportPosition.x > 1 && this.velocity.x>0) || (viewportPosition.x < 0 && this.velocity.x<0)))
		{

			newPosition.x = -newPosition.x;
			
			isWrappingX = true;
		}
		
		if (!isWrappingY && ((viewportPosition.y > 1 && this.velocity.y>0)|| (viewportPosition.y < 0 && this.velocity.y<0)))
		{
			newPosition.y = -newPosition.y;
			
			isWrappingY = true;
		}
		
		position = new Vector2(newPosition.x,newPosition.y) * scale;
		
		this.transform.position = newPosition;
	}

	public delegate void ShipCollidedHandler(Ship ship, GameObject other);
	public event ShipCollidedHandler ShipCollided;


	public void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "selection") { //this collision is coming from the child
			return;
		}
		Ship ship = other.gameObject.GetComponent<Ship>();
		if (ship != null) {
			//if((this.position - ship.position).magnitude>10000) return; //we're using 1 larger collider 

			//raise ship collision event
			ShipCollided(this,ship.gameObject);
		}
	}
}
