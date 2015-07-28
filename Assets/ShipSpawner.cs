using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipSpawner : MonoBehaviour {
	
	public SolarSystem solarSystem;
	private float elaspedTime = 0;
	private float nextTime = 0;

	public Vector2 CalculateScreenSizeInWorldCoords ()  {
		var cam = Camera.main;
		Vector3 p1 = cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane));  
		Vector3 p2 = cam.ViewportToWorldPoint(new Vector3(1,0,cam.nearClipPlane));
		Vector3 p3 = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane));
		
		float width = (p2 - p1).magnitude;
		float height = (p3 - p2).magnitude;
		
		Debug.Log (width + " " + height);
		
		Vector2 dimensions = new Vector2(width,height);
		
		return dimensions;
	}

	void PickPositionAndDirection( ref Vector2 position, ref Vector2 velocity) {
		Vector2 dimensions = CalculateScreenSizeInWorldCoords();
		
		int side = Random.Range(0,4);
		float x = 0;
		float y = 0;
		float delta = 2f;
		
		Vector2 accn = new Vector2();
		float velocityMagnitude = 50000f;
		if(side==0) {
			//going from top
			
			x =  Random.Range(-dimensions.x/2f,dimensions.x/2f);
			y = dimensions.y/2f + delta;
			accn = new Vector2(0,-velocityMagnitude);
			
			
		} else if(side==1){
			//going from right
			x = dimensions.x/2f + delta;
			y = Random.Range(-dimensions.y/2f,dimensions.y/2f);
			accn = new Vector2(-velocityMagnitude,0f);
			
		} else if(side==2){
			//going from bottom
			x = Random.Range(-dimensions.x/2f,dimensions.x/2f);
			y = -dimensions.y/2f -delta;
			accn = new Vector2(0f,velocityMagnitude);
		} else if(side==3){
			//going from left
			x = -dimensions.x/2f - delta;
			y = Random.Range(-dimensions.y/2f,dimensions.y/2f);
			accn = new Vector2(velocityMagnitude,0f);
		}
		Debug.Log ("spawn side is " + side);
		position.Set (x, y);

		velocity.Set(accn.x,accn.y);
		
	}

	bool HasClearPath(Vector2 position, Vector2 direction) {
		Debug.Log("testing path " + position.x + " " + position.y + " " +direction.x + " " + direction.y);
		Debug.DrawRay (new Vector3 (position.x, position.y, 0), direction, Color.green,2f);
		bool hasCollision = Physics2D.Raycast (new Vector3 (position.x, position.y), direction.normalized);
		Debug.Log (!hasCollision);
		return !hasCollision;
	}

	public void Spawn(Body ship) {
		Vector2 position = new Vector2 ();
		Vector2 velocity = new Vector2 ();
		PickPositionAndDirection (ref position, ref velocity);
		for(var i=0; i<10;i++) {
			if(!HasClearPath(position,velocity)) {
				PickPositionAndDirection (ref position, ref velocity);
			}else {
				float scale = 100000f;
				//ship.velocity = velocity;
				ship.AlignToVector(velocity);
				ship.position = position * scale;
				ship.lastPosition = ship.position - (velocity * Time.fixedDeltaTime);
				ship.gameObject.transform.position = new Vector3(-100,-100,0); //set start position to ensure z value is correct
				solarSystem.AddBody(ship);
				ship.gameObject.SetActive(true);
				break;
				
			}
		}
		
	}

	public delegate void SpawnedHandler(Body body);
	public event SpawnedHandler Spawned;
	
}
