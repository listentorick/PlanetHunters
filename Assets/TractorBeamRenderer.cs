using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OLDTractorBeamRenderer : MonoBehaviour {

	public SpriteRenderer sprite;
	public PolygonCollider2D target;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(target!=null) {
			
			//we center the sprite between the two objects
			Vector3 diff = target.gameObject.transform.position - this.transform.parent.transform.position;
			Vector3 direction = diff.normalized;
			Vector3 position = diff * (diff.magnitude/2f);
			sprite.transform.position = position;
			
		}
		
	}

	/*
	public PolygonCollider2D target;
	public ParticleSystem particleSystem;
	private MeshBuilder meshBuilder;
	public MeshFilter meshfilter;

	// Use this for initialization
	void Start () {
		meshBuilder = new MeshBuilder ();
	}
	
	// Update is called once per frame
	void Update () {
		RenderBeam ();
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

	private void RenderBeam() {
		
		if (target == null)
			return;
		// los angulos calcuados se encuentran en cuadrantes mixtos (1 y 4)
		bool lows = false; // check si hay menores a -0.5
		bool his = false; // check si hay mayores a 2.0
		List <verts> tempVerts = new List<verts>();
		
		
		
		//PolygonCollider2D p = target.GetRendererTransform ().gameObject.GetComponent<PolygonCollider2D> ();
		List<Vector3> points = new List<Vector3> ();
		for (int i = 0; i < target.GetTotalPointCount(); i++) {								
			Vector3 worldPoint = target.transform.TransformPoint(target.points[i]);
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
		
		
//		Debug.DrawLine (transform.position, transform.TransformPoint(tempVerts [posLowAngle].pos), Color.red);
//		Debug.DrawLine (transform.position, transform.TransformPoint(tempVerts [posHighAngle].pos),Color.red);
//MeshHelper
		//this.transform.LookAt (target.gameObject.transform.position);
		//this.particleSystem.
	//	meshBuilder.Vertices.Add(transform.position);
	//	meshBuilder.Vertices.Add(transform.TransformPoint(tempVerts [posLowAngle].pos));
	//	meshBuilder.Vertices.Add(transform.TransformPoint(tempVerts [posHighAngle].pos));

		meshBuilder.Vertices.Clear ();
		meshBuilder.Indices.Clear ();
		meshBuilder.UVs.Clear ();
		//meshBuilder.

		meshBuilder.Vertices.Add(tempVerts [posLowAngle].pos);
		meshBuilder.Vertices.Add(tempVerts [posHighAngle].pos);
		meshBuilder.Vertices.Add(new Vector3());

		meshBuilder.AddTriangle(0,1,2);

		meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
		meshBuilder.UVs.Add(new Vector2(1f, 0f));


		meshBuilder.UVs.Add(new Vector2(0.5f, 1f));
		//meshBuilder.UVs.Add(new Vector2(0f, 1.0f));
		//meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
		//Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;
		//meshBuilder.Normals.Add(normal);

		
		Mesh m = meshBuilder.CreateMesh ();
		meshfilter.mesh = m;
		//meshfilter.

	}*/
}
