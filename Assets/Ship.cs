using UnityEngine;
using System.Collections;

public enum Cargo {

	Food, Medical, Technology, People
}

public class Ship : Body {

	public int cargo;
	public int maxCargo;
	public Cargo cargoType;

	public ParticleSystem smokePrefab;
	public ParticleSystem thrusterPrefab;

	private ParticleSystem smoke;
	private ParticleSystem thruster;


	public AudioClip thrusterSound;
	public AudioSource audioSource;
	//public AudioClip explosionSound;

	private float rechargeTime = 0;
	public float fuelRechargeRate = 0.1f;
	public float fuel = 1f;
	public float burnRate = 0.0025f;
	public float hull = 1f;
	public bool takesDamage = false;
	float timeToNext = 0;
	protected bool thrustersActive = false;

	public void Thrust(Vector2 thrust){

		if (isExploding==true)
			return;

		thrustersActive = thrust.magnitude!=0;
		additionalForce = thrust;

		if (!thrustersActive) {
			timeToNext = 0.25f + Time.time;
			thruster.Stop();
			return;
		}

		if (thrustersActive) {
			thruster.Play();
			//if( !audioSource.isPlaying) {
				audioSource.volume = 1f;
				audioSource.clip = thrusterSound;
				audioSource.Play();
			//}
		}


	}
	
	private float timeInHighGravity = 0f;

	private bool hullFailureHandled = false;

	public void Update() {
	
		if (isExploding == false && thrustersActive == false && audioSource != null) {
			float vol = Mathf.Lerp (1f, 0f, (Time.time - timeToNext) / 0.10f);
			audioSource.volume = vol;
			if (audioSource.volume == 0) {
				//audioSource.Stop ();
			}
		} 
		if (isExploding) {
			audioSource.volume = 1f;
		}

		if (additionalForce != Vector2.zero && canMove) {
			fuel -= burnRate;
			if(fuel<0){

				thrustersActive = false;
				additionalForce = Vector2.zero;
				thruster.Stop();
				fuel = 0;
				rechargeTime =0;
			} else {
				AlignToVector(additionalForce);
			}
		} else {
			//not firing thrusters so can recharge
			if(rechargeTime>2f){ 
				rechargeTime = 0f; //reset recharge time.
				fuel += fuelRechargeRate;
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

			if (hull <= 0 && hullFailureHandled ==false) {
				hullFailureHandled = true;
				HullFailure (this);
			}
		}

		if (hull < 0.5f) {

			//Vector3 diff = this.lastPosition-this.position;
			//float angle = Mathf.Atan2 (diff.y, diff.x) * Mathf.Rad2Deg;
			//smoke.gameObject.transform.rotation = Quaternion.AngleAxis(angle + 90,Vector3.up);
			//smoke.gameObject.transform.localRotation = Quaternion.AngleAxis(angle,Vector3.forward);

			//Vector3 diff = this.position - this.lastPosition;   
			//float angle = Mathf.Atan2 ( diff.y, diff.x );
			//smoke.gameObject.transform.parent.transform.localRotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg );

			//smoke.gameObject.transform.rotation.eulerAngles = new Vector3(90,

			//smoke.gameObject.transform.LookAt(
			smoke.Play ();
		
		} else {
			if(smoke!=null)smoke.Stop();
		}

		base.Update ();

		rechargeTime += Time.deltaTime;
		//thrustersActive = false;
	}

	public void LateUpdate() {


	}



	public delegate void ShipCollidedHandler(Ship ship, Body other);
	public event ShipCollidedHandler ShipCollided;

	public delegate void HullFailureHandler(Ship ship);
	public event HullFailureHandler HullFailure;

	bool isExploding = false;

	public void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "selection") { //this collision is coming from the child
			return;
		}

		Body body = other.gameObject.GetComponent<Body>();

		if (body != null) {

			//isExploding = true;
			audioSource.Stop ();
			//audioSource.volume = 1f;
			//audioSource.clip = explosionSound;
			thrustersActive = false;
			//audioSource.PlayOneShot (explosionSound, 1f);
			//raise ship collision event
			ShipCollided (this, body);
		}

	}

	public void Start(){
		smoke = (ParticleSystem)Instantiate (smokePrefab);
		thruster = (ParticleSystem)Instantiate (thrusterPrefab);
		smoke.transform.parent = this.GetRendererTransform ();
		smoke.transform.localPosition = Vector3.zero;
		smoke.transform.localRotation = Quaternion.Euler (new Vector3 (90f, 0, 0));
		thruster.transform.parent = this.GetRendererTransform ();
		thruster.transform.localPosition = Vector3.zero;
		thruster.transform.localRotation = Quaternion.Euler(new Vector3(90f,0,0));


	}
}

