using UnityEngine;
using System.Collections;

public class BulletController : BodyController {

	public Bullet bulletPrefab;

	
	public override Body BuildBody(){
		Body bullet = (Body)Instantiate (bulletPrefab);
		return bullet;
	}
	
	public override Body ConfigureBody(Body b){
		return b;
	}
}
