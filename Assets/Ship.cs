using UnityEngine;
using System.Collections;

public enum Cargo {

	Food, Medical, Technology, People
}

public class Ship : Body {


	private float rechargeTime = 0;
	public float fuel = 1f;
	public float burnRate = 0.0025f;
	public float hull = 1f;
	public bool takesDamage = false;
	
	public void Thrust(Vector2 thrust){
		if (fuel <= 0)
			return;
		additionalForce = thrust;
	}
	
	private float timeInHighGravity = 0f;

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

		//ships under strong acceleration are basically fooked
		if (this.acceleration.magnitude > 10000f) {
			timeInHighGravity+=Time.deltaTime;
		}

		//should this be a component?
		if (takesDamage == true) {
			if (timeInHighGravity > 5f) {
				Debug.Log ("damage");
				timeInHighGravity = 0;
				hull -= 0.1f;
			}

			if (hull < 0) {
				HullFailure (this);
			}
		}

		base.Update ();

		rechargeTime += Time.deltaTime;
	}





	public delegate void ShipCollidedHandler(Ship ship, GameObject other);
	public event ShipCollidedHandler ShipCollided;

	public delegate void HullFailureHandler(Ship ship);
	public event HullFailureHandler HullFailure;


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
