using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SolarSystem : MonoBehaviour {

	List<Body> bodies = new List<Body> ();
	Vector2[,] forces = new Vector2[10,10];
	//List<List<Vector2>> forces = new List<List<Vector2>> ();
	Dictionary<Body,Vector2> preAcc = new Dictionary<Body,Vector2>();


	private float G = 6.67f * UnityEngine.Mathf.Pow(10,-11);

	public void AddBody(Body body){
		bodies.Add (body);
		//forces.Add (new List<Vector2>());
	}

	public void RemoveBody(Body body) {
		//do this at the end of the update cycle?
		bodies.Remove (body);
	}

	public void Clear(){
		bodies.Clear ();
	}
	
	private Vector2 CalculateForce(Body body1, Body body2) {  //vector force on body 1 from body 2
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



				//this.forces[i][i] = zeroForce;  //diagonal zeroForce forces now set in setN(N)
				for(var j = i+1; j < bodies.Count; j++){

					Vector2 force =  this.CalculateForce(bodies[i],bodies[j]);

					forces[i,j] = force;
					forces[j,i] = new Vector2(-force.x, -force.y);

				}

		}
	}

	public void Update () {
		UpdateForces(Time.deltaTime);
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
			if(bodies[i]!=b && IsInSOI(bodies[i],b)){
				return bodies[i];
			}
		}
		return null;
	}

	public bool IsInSOI(Body parent, Body child) {
		float distance = UnityEngine.Mathf.Pow (child.position.x - parent.position.x, 2) + UnityEngine.Mathf.Pow (child.position.y - parent.position.y, 2);
		return  distance< UnityEngine.Mathf.Pow (parent.soi, 2);
	}


	public delegate void ShipEnteredOrbitHandler(Body s, Body p);
	public event ShipEnteredOrbitHandler ShipEnteredOrbit;

	private void UpdateForces (float dt) { //delta is the num of milliseconds which have passed between updates
		
		//Body body;
		Vector2 pos;
		Vector2 acc;
		Vector2 vel;
		//Vector2 accCopy;

		foreach (Body body in bodies) {
			//for(var i=0; i< bodies.length;i++ ){
			//body.inOrbit = false;
			//body.justEnteredOrbit = false;
			if(body.canMove){
				Body SOIParent= GetParentBody(body);
				if(SOIParent!=null){
				
					if(body.inOrbit==false) {
						body.justEnteredOrbit = true;

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
					//body.parentBody=null;
					body.inOrbit = false;
					//body.justEnteredOrbit = false;
				}

			}

			//body = bodies[i];
			pos = body.position;
			vel = body.velocity;
			acc = body.acceleration;
			//accCopy = 
			preAcc[body] = new Vector2 (acc.x, acc.y);
			//pos.elements[0] = pos.elements[0] + vel.elements[0]*dt + (0.5)*acc.elements[0]*dt*dt;
			//pos.elements[1] = pos.elements[1] + vel.elements[1]*dt + (0.5)*acc.elements[1]*dt*dt;
			//body.preAcc = accCopy;
			float x = pos.x + vel.x * dt + (0.5f) * acc.x * dt * dt;
			float y = pos.y + vel.y * dt + (0.5f) * acc.y * dt * dt;
			if(body.canMove) {
				body.position.Set (x, y);
			}
		}
		
		UpdateForcesAndAccels();
		

		//update accelerations of bodies
		for (var n = 0; n < bodies.Count; n++) {
			bodies [n].acceleration.x = 0;
			bodies [n].acceleration.y = 0;
			var massN = bodies [n].mass;



			
			for (var m = 0; m < bodies.Count; m++) {

				if(bodies[n].inOrbit) {
				

					Vector2 f = CalculateForce(bodies[n],bodies[n].parentBody);
					bodies [n].acceleration.x = f.x / massN;
					bodies [n].acceleration.y = f.y / massN; 

					if(bodies[n].justEnteredOrbit){
						float radius = Vector2.Distance( bodies[n].position,bodies[n].parentBody.position);
						// (-dy, dx) and (dy, -dx).
						//calculate the desired init velocity to maintain orbit
						float v = UnityEngine.Mathf.Sqrt(G * bodies[n].parentBody.mass /radius );
						Vector2 vvector = new Vector2(f.normalized.y,-f.normalized.x) * v;

						//The velocity should be directed at 90s to the direction of the force...

						//Direction of velocity should be 90 degrees
						bodies [n].velocity.x = vvector.x;
						bodies [n].velocity.y = vvector.y;
						preAcc[bodies[n]] = new Vector2(0,0);
						bodies[n].justEnteredOrbit = false;
						bodies[n].additionalForce = new Vector2(0,0);
					}


				} else {

					bodies [n].acceleration.x += forces [n,m].x / massN;
					bodies [n].acceleration.y += forces [n,m].y / massN;    

				}

				//move this out?
				if(bodies[n].additionalForce!=null){
					
					bodies [n].acceleration.x +=bodies [n].additionalForce.x/massN;
					bodies [n].acceleration.y +=bodies [n].additionalForce.y/massN;
					
				}
			}
		}

		//Vector2 preAcc;
		for (var i=0; i< bodies.Count; i++) {
			
		
			vel = bodies [i].velocity;
			acc = bodies [i].acceleration;
			//preAcc = preAcc[bodies [i]];
			
			bodies [i].velocity.x = vel.x + (0.5f) * (acc.x + preAcc[bodies [i]].x) * dt;
			bodies [i].velocity.y = vel.y + (0.5f) * (acc.y + preAcc[bodies [i]].y) * dt;
			
			
		}
	}

}


	
	
	

