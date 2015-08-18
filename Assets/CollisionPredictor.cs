using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionPredictor : MonoBehaviour {


	private SolarSystem solarSystem;
	private Body thisBody;
	public float minTime = 5f;
	private SpriteRenderer renderer;
 	void Start() {
		solarSystem = FindObjectOfType<SolarSystem> ();
		thisBody = this.GetComponent<Body> ();
		renderer = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
	}

	float PredictNearestApproachTime(Body other) {
	
		Vector2 relativeVelocity = (other.velocity - thisBody.velocity);
		float relativeSpeed = relativeVelocity.magnitude;

		//detect parrallel paths
		if(Mathf.Approximately(relativeSpeed,0)) {
			return 0;
		}

		// Now consider the path of the other vehicle in this relative
		// space, a line defined by the relative position and velocity.
		// The distance from the origin (our vehicle) to that line is
		// the nearest approach.
		
		// Take the unit tangent along the other vehicle's path
		var relTangent = relativeVelocity / relativeSpeed;
		
		// find distance from its path to origin (compute offset from
		// other to us, find length of projection onto path)
		var relPosition = thisBody.position - other.position;
		var projection = Vector2.Dot(relTangent, relPosition);
		
		return projection / relativeSpeed;
	}

	public float ComputeNearestApproachPositions(Body other, float time)
	{
		var myTravel =  thisBody.velocity * time;
		var otherTravel = other.velocity * time;
		
		Vector2 ourPosition = this.thisBody.position + myTravel;
		Vector2 hisPosition = other.position + otherTravel;
		
		return Vector2.Distance(ourPosition, hisPosition);
	}


	//public delegate void CollisionPredictedHandler(Ship ship, Ship other);
	//public event CollisionPredictedHandler CollisionPredicted;


	// Update is called once per frame
	void Update () {

		renderer.material.color = new Color(1f, 1f, 1f, 1f); // Set to opaque black

		foreach (Body other in solarSystem.bodies) {
		
			//we only care about other bodies
			if(other!=thisBody) {

				float time = this.PredictNearestApproachTime (other);

				
				// If the time is in the future, sooner than any other
				// threatened collision...
				if ((time >= 0) && (time < minTime))
				{
					Vector2 ourPos = Vector2.zero;
					Vector2 hisPos = Vector2.zero;
					float	dist   = this.ComputeNearestApproachPositions (other, time);

					if(other is Planet){

						if (dist <100000f &&  thisBody.velocity.magnitude > SolarSystem.MAX_RENTRY_SPEED){
							renderer.material.color = new Color(1f, 0f, 0f, 1f); 
						}
					} else if (other is Ship) {
				
						if(dist <100000f) {

							renderer.material.color = new Color(1f, 0f, 0f, 1f); 
							Debug.Log("collision" + dist);
						
						}
					}
				}
			
			}


		
		}
	
	}
}
