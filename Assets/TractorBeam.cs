using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TractorBeam : MonoBehaviour {

	public SolarSystem solarSystem;
	public Body parent;
	public float distance;
	public Body connectedBody;
	public TractorBeamRenderer tractorBeamRenderer;

	// Use this for initialization
	void Start () {
		solarSystem = FindObjectOfType<SolarSystem> ();
		solarSystem.ConnectionBroken+= HandleConnectionBroken;

		//needs to knwow where other ships are within a 
		//needs to draw triangle from ship to extents of other ship.
		//if I'm moving away from the ship apply a force to the other ship (what ever force is acting on us)
		//if i'm moving towards it do nothing
	
	}

	void HandleConnectionBroken (Body s, Body p)
	{
		//this ship owned the connection
		if ((s == parent && p == connectedBody) || (s== connectedBody || p == parent)) {
			//the connected body
			//if(s== connectedBody || p == connectedBody){
				connectedBody = null;
				connection = false;
			//}
		}
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


	private void RenderBeam() {
	
		if (connectedBody == null)
			return;
		// los angulos calcuados se encuentran en cuadrantes mixtos (1 y 4)
		bool lows = false; // check si hay menores a -0.5
		bool his = false; // check si hay mayores a 2.0
		List <verts> tempVerts = new List<verts>();

		

		PolygonCollider2D p = connectedBody.GetRendererTransform ().gameObject.GetComponent<PolygonCollider2D> ();
		List<Vector3> points = new List<Vector3> ();
		for (int i = 0; i < p.GetTotalPointCount(); i++) {								
			Vector3 worldPoint = p.transform.TransformPoint(p.points[i]);
			points.Add(worldPoint);

			verts v = new verts();

			v.pos = transform.InverseTransformPoint(worldPoint); 
			v.angle = getVectorAngle(true,v.pos.x, v.pos.y);

			// -- bookmark if an angle is lower than 0 or higher than 2f --//
			//-- helper method for fix bug on shape located in 2 or more quadrants
			if(v.angle < 0f )
				lows = true;
			
			if(v.angle > 2f)
				his = true;

			tempVerts.Add(v);

		}

		sortList(tempVerts); // sort first

		int posLowAngle = 0; // save the indice of left ray
		int posHighAngle = 0; // same last in right side
		
		//Debug.Log(lows + " " + his);
		
		if(his == true && lows == true){  //-- FIX BUG OF SORTING CUANDRANT 1-4 --//
			float lowestAngle = -1f;//tempVerts[0].angle; // init with first data
			float highestAngle = tempVerts[0].angle;
			
			
			for(int d=0; d<tempVerts.Count; d++){
				
				
				
				if(tempVerts[d].angle < 1f && tempVerts[d].angle > lowestAngle){
					lowestAngle = tempVerts[d].angle;
					posLowAngle = d;
				}
				
				if(tempVerts[d].angle > 2f && tempVerts[d].angle < highestAngle){
					highestAngle = tempVerts[d].angle;
					posHighAngle = d;
				}
			}
			
			
		}else{
			//-- convencional position of ray points
			// save the indice of left ray
			posLowAngle = 0; 
			posHighAngle = tempVerts.Count-1;
			
		}

		//tempVerts[posLowAngle].location = 1; // right
		//tempVerts[posHighAngle].location = -1; // left


		Debug.DrawLine (transform.position, transform.TransformPoint(tempVerts [posLowAngle].pos), Color.red);
		Debug.DrawLine (transform.position, transform.TransformPoint(tempVerts [posHighAngle].pos),Color.red);


	}

	void sortList(List<verts> lista){
		lista.Sort((item1, item2) => (item2.angle.CompareTo(item1.angle)));
	}

	float getVectorAngle(bool pseudo, float x, float y){
		float ang = 0;
		if(pseudo == true){
			ang = pseudoAngle(x, y);
		}else{
			ang = Mathf.Atan2(y, x);
		}
		return ang;
	}

	float pseudoAngle(float dx, float dy){
		// Hight performance for calculate angle on a vector (only for sort)
		// APROXIMATE VALUES -- NOT EXACT!! //
		float ax = Mathf.Abs (dx);
		float ay = Mathf.Abs (dy);
		float p = dy / (ax + ay);
		if (dx < 0){
			p = 2 - p;
			
		}
		return p;
	}

	// Update is called once per frame
	void Update () {
		//RenderBeam ();
		Body closestBody = GetClosestBody ();
		if (closestBody != null && connection == false) {
			connection = true;
			connectedBody  = closestBody;

			tractorBeamRenderer.target = connectedBody.GetRendererTransform ().gameObject.GetComponent<PolygonCollider2D> ();

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
