using UnityEngine;
using System.Collections;

public class Turret : Ship {

	private SolarSystem sol;
	//public BulletController prefab;
	private BulletController bulletController;


	public void Start(){
		sol = FindObjectOfType<SolarSystem> ();
		bulletController = FindObjectOfType<BulletController> ();

	}
	private float lastShotTime = float.MinValue;
	public float bulletSpeed = 1000f;
	//public bool hasTarget;
	public void Update(){

		Body target = null;
		//hasTarget = false;

		foreach (Body b in sol.bodies) {

			if(b==this) continue;
			if(!(b is Ship)){
				continue;
			}

			if(Vector2.Distance(this.position,b.position)<4000000){

			
				target = b;
			
				SmoothLookAt(b.gameObject.transform.position,30f);
				//AlignToVector(direction);
				//hasTarget = true;
				break;
			}
		}

		if (target!=null) {

			if(Time.time>lastShotTime + 2f){


				lastShotTime = Time.time;

				Vector2 direction = (target.position- this.position).normalized;

				Vector2 position = this.position + (direction *bulletSpeed * Time.fixedDeltaTime);


				bulletController.Spawn(position/GameController.SCALE,direction * bulletSpeed);
			}
		}


		base.Update ();
	}
}
