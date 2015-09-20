using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public struct Link{
	public Body body1;
	public Body body2;
	public float length;
	public float stiffness;
	public Link (Body b1, Body b2, float len, float stiffness){
		body1 =b1;
		body2 = b2;
		length = len;
		this.stiffness = stiffness;
	}
}



public class SolarSystem : MonoBehaviour, IStartStop, IReset {

	public static float MAX_RENTRY_SPEED = 250000; 
	public static float MAX_FORCE = 1000000; 


	//adding wrapping by specifying world bounds
	private bool stop = true;
	public void StopPlay(){
		stop = true;
	}

	public void StartPlay(){
		stop = false;
	}

	public void Reset(){
		stop = true;
	}

	private Vector2 worldBounds;

	public void SetWorldBounds(Vector2 worldBounds) {
	
		this.worldBounds = worldBounds;
	
	}

	public List<Body> bodies = new List<Body> ();
	Vector2[,] forces = new Vector2[50,50];
	//List<List<Vector2>> forces = new List<List<Vector2>> ();
	Dictionary<Body,Vector2> preAcc = new Dictionary<Body,Vector2>();


	private float G = 6.67f * UnityEngine.Mathf.Pow(10,-11);

	public void AddBody(Body body){
		bodies.Add (body);
		//forces.Add (new List<Vector2>());
	}


	private List<Link> links = new List<Link>();

	public void AddConnection(Body b1, Body b2, float length, float springConstant)
	{

		links.Add (new Link(b1, b2, length,springConstant));
	
	}

	public void RemoveConnectionsForBody(Body b) {
		List<Link> connectionsToRemove = new List<Link> ();
		foreach (Link l in links) {
			if(l.body1 == b || l.body2 ==b){
				connectionsToRemove.Add(l);
			}
		}
		foreach (Link l in connectionsToRemove) {
			links.Remove(l);
			ConnectionBroken(l.body1,l.body2);
		}
	}

//	public Bo

	public void RemoveBody(Body body) {
		Debug.Log ("removing a body");
	
		this.RemoveConnectionsForBody (body);
		//do this at the end of the update cycle?
		bodies.Remove (body);
	}

	public void Clear(){
		links.Clear ();
		bodies.Clear ();
	}
	
	public Vector2 CalculateForce(Body body1, Body body2) {  //vector force on body 1 from body 2
		var GM1M2 = G*body1.mass*body2.mass;
		var delX = body2.position.x - body1.position.x;    //x2 - x1;
		var delY = body2.position.y - body1.position.y;    //y2 - y1;
		var distSq = delX*delX + delY*delY;
		var dist = UnityEngine.Mathf.Sqrt(distSq);
		float product = GM1M2/(distSq*dist);  
		var force = new Vector2 (product*delX,product*delY);

		return force;
	}



	private void UpdateForcesAndAccels () { //compute current forces and  acceleration of all bodies in system
		//update forces matrix
		//var zeroForce:Vector = new Vector(0,0); 
		//indices i and j start at 0!
		for (var i = 0; i < bodies.Count ; i++){

			for(var j = i+1; j < bodies.Count; j++){

				//scale force
				Vector2 force =  this.CalculateForce(bodies[i],bodies[j]);
				if(force.magnitude>MAX_FORCE){
					force = force * (MAX_FORCE/force.magnitude);
				}

				forces[i,j] = force;
				forces[j,i] = new Vector2(-force.x, -force.y);

			}

		}
	
	}
	
	public void FixedUpdate() {
		if (stop)
			return;
		UpdateForces (Time.fixedDeltaTime);
	}

	public Body ClosestBody(Body body) {
		Body closest = null;
		float minDistance=1000000000000000f;
		float currentDistance = 0f;
		foreach (Body b in bodies) {
			currentDistance = Vector2.Distance(b.position,body.position);
			if(currentDistance<minDistance) {
				minDistance = currentDistance;
				closest = b;
			}
		}
		return closest;
	}

	//private float radius =10000f;

	public Body GetParentBody(Body b) {
		for(int i=0; i<this.bodies.Count;i++){
			if(bodies[i]!=b && IsInSOI(bodies[i],b.position)){
				return bodies[i];
			}
		}
		return null;
	}

	public bool IsInAnySOI(Vector2 childPosition) {
		foreach (Body parent in bodies) {
			if(parent.canMove==false && IsInSOI(parent,childPosition)==true){
				return true;
			}
		}
		return false;
	}


	public bool IsInSOI(Body parent, Vector2 childPosition) {
		float distance = UnityEngine.Mathf.Pow (childPosition.x - parent.position.x, 2) + UnityEngine.Mathf.Pow (childPosition.y - parent.position.y, 2);
		return  distance< UnityEngine.Mathf.Pow (parent.soi, 2);
	}


	public delegate void ShipEnteredOrbitHandler(Body s, Body p);
	public event ShipEnteredOrbitHandler ShipEnteredOrbit;

	public delegate void ConnectionBrokenHandler(Body s, Body p);
	public event ConnectionBrokenHandler ConnectionBroken;


	public Vector2 CalculateForceAtPoint(Vector2 position) {
		Vector2 force = Vector2.zero;
		int numBodies = bodies.Count;
		for(var j = 0; j < numBodies; j++){
			force +=  this.CalculateForce(position,1f,bodies[j].position, bodies[j].mass);
		}
		if(force.magnitude>SolarSystem.MAX_FORCE){
			force = force * (SolarSystem.MAX_FORCE/force.magnitude);
		}

		return force;
	}

	private Vector2 CalculateForce(Vector2 position1, float mass1, Vector2 position2, float mass2) {  //vector force on body 1 from body 2
		var GM1M2 = G*mass1*mass2;
		var delX = position2.x - position1.x;    //x2 - x1;
		var delY = position2.y - position1.y;    //y2 - y1;
		var distSq = delX*delX + delY*delY;
		var dist = UnityEngine.Mathf.Sqrt(distSq);
		float product = GM1M2/(distSq*dist);  
		var force = new Vector2 (product*delX,product*delY);
		return force;
	}


	float tearSensitivity =100000f;
	private void SolveLinks(){
		foreach (Link l in links) {

			Vector2 distance = l.body1.position - l.body2.position;

			float d = distance.magnitude;

			// find the difference, or the ratio of how far along the restingDistance the actual distance is.
			float difference = (l.length - d) / d;

			// Inverse the mass quantities
			float im1 = 1 / l.body1.mass;
			float im2 = 1 / l.body2.mass;
			float scalarP1 = (im1 / (im1 + im2)) * l.stiffness;
			float scalarP2 = l.stiffness - scalarP1;
			
			// Push/pull based on mass
			// heavier objects will be pushed/pulled less than attached light objects
			l.body1.position+= distance * scalarP1 * difference;
			l.body2.position -= distance * scalarP2 * difference;

		}
	
	}


	private void UpdateForces (float dt) { //delta is the num of milliseconds which have passed between updates
		

		Vector2 pos;
		Vector2 acc;
		Vector2 vel;

		foreach (Body body in bodies) {
			if(body.canMove){
				Body SOIParent= GetParentBody(body);
				if(SOIParent!=null){
					
					if(body.inOrbit==false) {
						body.justEnteredOrbit = true;

						//add a link
						this.RemoveConnectionsForBody (body);
						this.AddConnection(SOIParent, body,(body.position-SOIParent.position).magnitude,1f);


						Vector2 f = CalculateForce(body,SOIParent);
						float radius = Vector2.Distance( body.position,SOIParent.position);
						float v = UnityEngine.Mathf.Sqrt(G * SOIParent.mass /radius );
						Vector2 vvector = new Vector2(f.normalized.y,-f.normalized.x) * v;
						body.lastPosition = body.position - vvector * dt;


						if(ShipEnteredOrbit != null)
						{
							// All listeners will be invoked
							ShipEnteredOrbit(body,SOIParent);
						}
						
						
					} else {
						body.justEnteredOrbit = false;
					}
					body.inOrbit = true;
					body.parentBody = SOIParent;
					//object should now just stop..
					//continue;
				}else {
	
					body.inOrbit = false;
				}
				
			}

			for (int i=0; i<10; i++) { 
				SolveLinks ();
			}
		}
		
		UpdateForcesAndAccels();
		
		
		//update accelerations of bodies
		for (var n = 0; n < bodies.Count; n++) {
			bodies [n].acceleration.x = 0;
			bodies [n].acceleration.y = 0;
			var massN = bodies [n].mass;

			for (var m = 0; m < bodies.Count; m++) {

				bodies [n].acceleration.x += forces [n,m].x / massN;
				bodies [n].acceleration.y += forces [n,m].y / massN;    
					

				//move this out?
				if(bodies[n].additionalForce!=null && bodies[n].additionalForce.magnitude>0){
					if(bodies[n].inOrbit) this.RemoveConnectionsForBody (bodies[n]);
					//if(bodies[n].parentBody && 
					bodies [n].acceleration.x +=bodies [n].additionalForce.x/massN;
					bodies [n].acceleration.y +=bodies [n].additionalForce.y/massN;
					
				}
			}
		}
		
		//Vector2 preAcc;
		for (var i=0; i< bodies.Count; i++) {

			if(bodies[i].canMove) {

				vel = bodies[i].position - bodies[i].lastPosition;
				bodies[i].velocity = vel/dt;
				float timeStepSq = dt * dt;
				
				// calculate the next position using Verlet Integration
				float nextX  = bodies[i].position.x +  vel.x + 0.5f * bodies [i].acceleration.x * timeStepSq;
				float nextY  =  bodies[i].position.y +  vel.y + 0.5f * bodies [i].acceleration.y * timeStepSq;

				
				// reset variables
				bodies [i].lastPosition  = new Vector2(bodies [i].position.x,bodies [i].position.y);

				//Lets check this new position against the world bounds...


				bodies[i].position = new Vector2(nextX,nextY);
				Wrap (bodies[i]);

			}

		}
	}

	private List<Body> wrappingBodiesX = new List<Body>();
	private List<Body> wrappingBodiesY = new List<Body>();


	private bool IsWrappingX(Body b){
		return wrappingBodiesX.Contains (b);
	}

	private bool IsWrappingY(Body b){
		return wrappingBodiesY.Contains (b);
	}


	private void Wrap(Body b) {
	
		if (b.IsRendererVisible ()) {
			if(IsWrappingX(b)){
				wrappingBodiesX.Remove(b);
			}
			if(IsWrappingY(b)){
				wrappingBodiesY.Remove(b);
			}
			return;
		}

		if (IsWrappingX (b) && IsWrappingY (b)) {
			return;
		}

		//var newPosition = transform.position;
		var velocity = b.position - b.lastPosition;

		if (!IsWrappingX (b)){

			if((b.position.x> (worldBounds.x/2f) && velocity.x>0)  || (b.position.x < (-worldBounds.x/2f) && velocity.x <0)) {
				wrappingBodiesX.Add(b);
				b.position.x = -b.position.x;
				b.lastPosition.x = b.position.x - velocity.x;
				this.RemoveConnectionsForBody(b);
			}
		}

		if (!IsWrappingY (b)){
			
			if((b.position.y> (worldBounds.y/2f) && velocity.y>0)  || (b.position.y < (-worldBounds.y/2f) && velocity.y <0)) {
				wrappingBodiesY.Add(b);
				b.position.y = -b.position.y;
				b.lastPosition.y = b.position.y - velocity.y;

				this.RemoveConnectionsForBody(b);
			}
		}
	
	}

}


	
	
	

