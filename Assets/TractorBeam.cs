using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TractorBeam : MonoBehaviour {

	public SolarSystem solarSystem;
	public Body parent;
	public float distance;
	public Body connectedBody;
	// Use this for initialization
	void Start () {
		solarSystem = FindObjectOfType<SolarSystem> ();

		//needs to knwow where other ships are within a 
		//needs to draw triangle from ship to extents of other ship.
		//if I'm moving away from the ship apply a force to the other ship (what ever force is acting on us)
		//if i'm moving towards it do nothing
	
	}

	IList<Body> ships = new List<Body>();

	public Body GetClosestBody() {
		foreach (Body b in solarSystem.bodies) { 
			
			if(b is ColonyShip && b!=parent){
				float seperation = (b.position - parent.position).magnitude;
				if(seperation < distance){
					return b;
				}
			}
		}
		return null;
	}

	bool IsMovingTowards(Body b) {
	
		Vector2 vectorFromParentToB = (b.position - parent.position).normalized;
		return Vector2.Dot (vectorFromParentToB, parent.acceleration) > 0;
	
	}

	public bool connection = false;
	
	// Update is called once per frame
	void Update () {
		connectedBody = GetClosestBody ();
		if (connectedBody != null && connection == false) {
			connection = true;

			//thoughts
			//1) Once within soi becomes increasingly difficult for the object to move

			//OR

			//Look at this
			//http://nehe.gamedev.net/tutorial/rope_physics/17006/
				
			//apply a force towards 
		//	Vector2 direction = (parent.position - connectedBody.position);

			solarSystem.AddConnection(parent, connectedBody,distance,1f);
			
			//if(direction.magnitude < (distance/2f)){

				//caught
				//connectedBody.additionalForce = Vector2.zero;
				//connectedBody.velocity = parent.velocity;
				//connectedBody.additionalForce = - direction.normalized * 50000f;
			//} else {
			  // connectedBody.additionalForce = direction.normalized * 50000f;

			//}

			//calculate repulsion
		//	Vector2 repulsion = - direction * (1f/(distance * distance));
		//	connectedBody.additionalForce+=repulsion;
		
		
		}
	}
}
