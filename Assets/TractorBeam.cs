﻿using UnityEngine;
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
	}

	void HandleConnectionBroken (Body s, Body p)
	{
		//this ship owned the connection
		if ((s == parent && p == connectedBody) || (s== connectedBody || p == parent)) {
			connectedBody = null;
			connection = false;
			tractorBeamRenderer.target = null;
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

	public void HighlightBodies() {
		foreach (Body b in solarSystem.bodies) { 
			
			if( b!=parent){
				float seperation = (b.position - parent.position).magnitude;
				if(seperation < distance){
					b.IsSelected = true;
				} else {
					b.IsSelected = false;
				}
			}
		}

	}

	bool IsMovingTowards(Body b) {
	
		Vector2 vectorFromParentToB = (b.position - parent.position).normalized;
		return Vector2.Dot (vectorFromParentToB, parent.acceleration) > 0;
	
	}

	public bool connection = false;

	/*
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
	}*/

	public Body GetBody(GameObject g){
	
		Body target = g.GetComponent<Body>();
		if (target == null) {
			target = g.GetComponentInParent<Body>();

		}

		return target;
	
	}

	public void TryTractor(GameObject g) 
	{
		Body target = GetBody (g);
		bool isDetaching = false;
		if (connectedBody != null) {
			if(connectedBody==target) {
				isDetaching = true;
			}
			connectedBody = null;
			connection = false;
			tractorBeamRenderer.target = null;
			solarSystem.RemoveConnectionsForBody (parent);
		} 

		if (isDetaching) {
			//The user has clicked an already connected object
			//They want to detach!
			return;
		}

		if (target != null) {
			float seperation = (target.position - parent.position).magnitude;
			if (seperation < distance) {
				connectedBody = target;
				connection = true;
				tractorBeamRenderer.target = connectedBody.gameObject.GetComponent<PolygonCollider2D> ();
				solarSystem.AddConnection (parent, connectedBody, seperation, 1f);

			}
		}
	}

	// Update is called once per frame
	void Update () {

		HighlightBodies ();
		//Body closestBody = GetClosestBody ();
		//if (closestBody != null && connection == false) {
		//	connection = true;
		//	connectedBody  = closestBody;

		//	tractorBeamRenderer.target = connectedBody.GetRendererTransform ().gameObject.GetComponent<PolygonCollider2D> ();
		//	solarSystem.AddConnection(parent, connectedBody,distance,1f);

		//}
	}
}
