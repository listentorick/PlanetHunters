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
	private Renderer[] renderers;

	public float fuel = 1f;
	public float burnRate = 0.01f;

	public void Start(){
		shipRendererTransform = this.gameObject.transform.GetChild (0);
		renderers = this.GetComponentsInChildren<Renderer> ();
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
				rechargeTime =0;
			}

			AlignToVector(additionalForce);
			//var angle = Mathf.Atan2 (additionalForce.y, additionalForce.x) * Mathf.Rad2Deg;

		//if(additionalForce.x==0 && additionalForce.y>0) {
				//pointing down
		//		angle += 180;
		//	}
		//	if(additionalForce.x==0 && additionalForce.y>0) {
		//		//pointing up
		//		angle += 180;
		//	}
			/*
			
			if (additionalForce.x < 0 && additionalForce.y > 0) {
				//topleft
				angle -= 90;
			}
			if (additionalForce.x > 0 && additionalForce.y > 0) {
				angle += 270;
			}
			if (additionalForce.x > 0 && additionalForce.y < 0) {
				angle -= 90;
			}
			if (additionalForce.x < 0 && additionalForce.y < 0) {
				angle += 270;
			}
			if (additionalForce.x >= 0 && additionalForce.y < 0) {
				angle -= 90;
			}
			if (additionalForce.x >= 0 && additionalForce.y > 0) {
				angle += 90;
			}
			
			shipRendererTransform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			*/
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
		foreach (Renderer r in renderers) {
			if(r.isVisible==true){
				return true;
			}
		}
		return false;
	}

	void ScreenWrap()
	{
	//	if (shipRendererTransform == null) {
	//		shipRendererTransform = this.gameObject.transform.GetChild (0);
		//}
		//shipRendererTransform.GetComponent<Sprite
		
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

		//could see if vector is towards boundardy, if so, we dont need to wrap
		//use a raycast? 

		//wrapping occurs in region after spawn area

		//if just spawned track this and when enter view port, remove is spawned
		//then can wrap

		//or only wrap if travell

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
		Ship ship = other.gameObject.GetComponent<Ship>();
		if (ship != null) {
			//raise ship collision event
			ShipCollided(this,ship.gameObject);
		}
	}
}
