using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Vectrosity;

public class ContourRenderer2D : ContourRenderer {

	public GravityFieldHelper fieldHelper;
	
	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private MeshBuilder meshBuilder;
	private MeshHelper meshHelper;

	// Use this for initialization
	void Start () {


	}

	float minForce = 10000000f;
	float maxForce = 0f;


	private Material lineMaterial;

	private void CreateLineMaterial() 
	{
		
		if( !lineMaterial ) {
			lineMaterial = new Material( "Shader \"Lines/Colored Blended\" {" +
			                            "SubShader { Pass { " +
			                            "    Blend SrcAlpha OneMinusSrcAlpha " +
			                            "    ZWrite Off Cull Off Fog { Mode Off } " +
			                            "    BindChannels {" +
			                            "      Bind \"vertex\", vertex Bind \"color\", color }" +
			                            "} } }" );
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;}
	}

	private Vector3[,] gravityPoints;
	public bool ready = false;
	private List<Vector3> lines;
	List<Mesh> meshes = new List<Mesh> ();
	Mesh uberMesh;
	public override void Build() {
		ready = true;
		gravityPoints = fieldHelper.CalculatePoints ();
		Debug.Log ("forces " + minForce +  " " + maxForce);
	
		//automate this... look at distribution of values...
		float[] zLevels = new float[19];
		zLevels [0] = 0;
		zLevels [1] = 1000;
		zLevels [2] = 2000;
		zLevels [3] = 3000;
		zLevels [4] = 4000;
		zLevels [5] = 5000;
		zLevels [6] = 6000;
		zLevels [7] = 7000;
		zLevels [8] = 8000;
		zLevels [9] = 9000;
		zLevels [10] = 10000;
		zLevels [11] = 15000;
		zLevels [12] = 20000;
		zLevels [13] = 25000;
		zLevels [14] = 30000;
		zLevels [15] = 35000;
		zLevels [16] = 40000;
		zLevels [17] = 45000;
		zLevels [18] = 50000;

		//lines = DrawContour (gravityPoints, zLevels);
		lines = DrawContour (gravityPoints, 0, 50000f, 20);
		//lines = DrawContour (gravityPoints, minForce, maxForce, 50);
		CreateLineMaterial ();

		//VectorLine l = new VectorLine("Graph", _pointsArray, this.color, null, 1.0f, LineType.Continuous, Joins.Weld);

		
		for (int i=0; i<lines.Count-1; i++) {
			//VectorLine l = new Vectorl( "Circle"+i, Vector3[new Vector3(lines[i].x,lines[i].y, 1),new Vector3(lines[i+1].x,lines[i+1].y,1)], Color.white, lineMaterial, 3, 0, 1, LineType.Continuous, Joins.Fill );


			Vector2 solPos = new Vector2(lines[i].x * 100000f,lines[i].y * 100000f);
			//if(!sol.IsInAnySOI(solPos)){
				VectorLine l = VectorLine.SetLine3D (new Color(0,1,0,0.1f), new Vector3(lines[i].x,lines[i].y, 1),new Vector3(lines[i+1].x,lines[i+1].y, 1));

			//}
			i++;
		}

		return;

		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();

		meshBuilder = new MeshBuilder();
		meshHelper = new MeshHelper ();
		Debug.Log (lines.Count);
		meshes.Clear ();
		// meshes = new List<Mesh> ();
		for(int i=0;i<lines.Count-1;i++){
			meshHelper.BuildLine(meshBuilder,lines[i],lines[i+1],0.1f);
			Mesh m = meshBuilder.CreateMesh();
			meshes.Add(m);
			meshBuilder.UVs.Clear();
			meshBuilder.Vertices.Clear();
			meshBuilder.Normals.Clear();
			i++;
		}

		//int i = 0;
		//while (i < meshFilters.Length) {
		//	combine[i].mesh = meshFilters[i].sharedMesh;
		//	combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
		//	meshFilters[i].gameObject.active = false;
		//	i++;
		//}
		CombineInstance[] combine = new CombineInstance[meshes.Count];
		for(int j=0;j<meshes.Count;j++){
			combine[j].mesh  = meshes[j];
			combine[j].transform = transform.localToWorldMatrix;

		}


		uberMesh = new Mesh();
		uberMesh.CombineMeshes(combine);
		meshFilter.mesh = uberMesh;

		//foreach (Mesh m in meshes) {
			
		//}



		//meshFilter.mesh = meshBuilder.CreateMesh();
	
	}

	private Color mainColor = new Color(0f,1f,0f,0.1f);

	void OnPostRender() {

	
		

		return;
		if (!ready) {
			return;
		}

		GL.PushMatrix();
		//GL.MultMatrix(this.transform.transform.localToWorldMatrix);
		//GL.Begin( GL.LINES );

		CreateLineMaterial();
		// set the current material
		lineMaterial.SetPass( 0 );
		
		GL.Begin( GL.LINES );

		GL.Color(mainColor);

		for(int i=0;i<lines.Count;i++){
			GL.Vertex3(lines[i].x,lines[i].y,0);
			//lineRenderer.SetPosition(i,lines[i]);
		}
		//DrawContour (gravityPoints, 1000, 100000, 80);

		GL.End();
		GL.PopMatrix();
		//ready = false;
	}

	private  List<Vector3> DrawContour( Vector3[,] pts, float[] zlevels)
	{
		List<Vector3> lineVertexes = new List<Vector3> ();
		
		//using (var aPen = new Pen(Color.DimGray) {Width = 0.25f})
		//{
		var pta = new Vector2[2];
		int ncount = zlevels.Length;
		

		int i0, i1, i2, j0, j1, j2;
		float zratio = 1; // Draw contour on the XY plane:
		for (int i = 0; i < pts.GetLength(0) - 1; i++)
		{
			for (int j = 0; j < pts.GetLength(1) - 1; j++)
			{
				for (int k = 0; k < ncount; k++)
				{
					// Left triangle:
					i0 = i;
					j0 = j;
					i1 = i;
					j1 = j + 1;
					i2 = i + 1;
					j2 = j + 1;
					if ((zlevels[k] >= pts[i0, j0].z && zlevels[k] < pts[i1, j1].z ||
					     zlevels[k] < pts[i0, j0].z && zlevels[k] >= pts[i1, j1].z) &&
					    (zlevels[k] >= pts[i1, j1].z && zlevels[k] < pts[i2, j2].z ||
					 zlevels[k] < pts[i1, j1].z && zlevels[k] >= pts[i2, j2].z))
					{
						zratio = (zlevels[k] - pts[i0, j0].z)/(pts[i1, j1].z - pts[i0, j0].z);
						pta[0] =
							new Vector2(pts[i0, j0].x, (1 - zratio)*pts[i0, j0].y + zratio*pts[i1, j1].y);
						zratio = (zlevels[k] - pts[i1, j1].z)/(pts[i2, j2].z - pts[i1, j1].z);
						pta[1] =
							new Vector2((1 - zratio)*pts[i1, j1].x + zratio*pts[i2, j2].x, pts[i1, j1].y);
						
						//	GL.Vertex3(pta[0].x,pta[0].y,0);
						//	GL.Vertex3(pta[1].x,pta[1].y,0);
						lineVertexes.Add(new Vector3(pta[0].x,pta[0].y,0));
						lineVertexes.Add(new Vector3(pta[1].x,pta[1].y,0));
						//g.DrawLine(aPen, pta[0], pta[1]);
					}
					else if ((zlevels[k] >= pts[i0, j0].z && zlevels[k] < pts[i2, j2].z ||
					          zlevels[k] < pts[i0, j0].z && zlevels[k] >= pts[i2, j2].z) &&
					         (zlevels[k] >= pts[i1, j1].z && zlevels[k] < pts[i2, j2].z ||
					 zlevels[k] < pts[i1, j1].z && zlevels[k] >= pts[i2, j2].z))
					{
						zratio = (zlevels[k] - pts[i0, j0].z)/(pts[i2, j2].z - pts[i0, j0].z);
						pta[0] =
							new Vector2((1 - zratio)*pts[i0, j0].x + zratio*pts[i2, j2].x,
							            (1 - zratio)*pts[i0, j0].y + zratio*pts[i2, j2].y);
						zratio = (zlevels[k] - pts[i1, j1].z)/(pts[i2, j2].z - pts[i1, j1].z);
						pta[1] =
							new Vector2((1 - zratio)*pts[i1, j1].x + zratio*pts[i2, j2].x, pts[i1, j1].y);
						
						//GL.Vertex3(pta[0].x,pta[0].y,0);
						//GL.Vertex3(pta[1].x,pta[1].y,0);
						
						lineVertexes.Add(new Vector3(pta[0].x,pta[0].y,0));
						lineVertexes.Add(new Vector3(pta[1].x,pta[1].y,0));
						//g.DrawLine(aPen, pta[0], pta[1]);
					}
					else if ((zlevels[k] >= pts[i0, j0].z && zlevels[k] < pts[i1, j1].z ||
					          zlevels[k] < pts[i0, j0].z && zlevels[k] >= pts[i1, j1].z) &&
					         (zlevels[k] >= pts[i0, j0].z && zlevels[k] < pts[i2, j2].z ||
					 zlevels[k] < pts[i0, j0].z && zlevels[k] >= pts[i2, j2].z))
					{
						zratio = (zlevels[k] - pts[i0, j0].z)/(pts[i1, j1].z - pts[i0, j0].z);
						pta[0] =
							new Vector2(pts[i0, j0].x, (1 - zratio)*pts[i0, j0].y + zratio*pts[i1, j1].y)
								;
						zratio = (zlevels[k] - pts[i0, j0].z)/(pts[i2, j2].z - pts[i0, j0].z);
						pta[1] =
							new Vector2(pts[i0, j0].x*(1 - zratio) + pts[i2, j2].x*zratio,
							            pts[i0, j0].y*(1 - zratio) + pts[i2, j2].y*zratio);
						
						//GL.Vertex3(pta[0].x,pta[0].y,0);
						//GL.Vertex3(pta[1].x,pta[1].y,0);
						
						lineVertexes.Add(new Vector3(pta[0].x,pta[0].y,0));
						lineVertexes.Add(new Vector3(pta[1].x,pta[1].y,0));
						//g.DrawLine(aPen, pta[0], pta[1]);
					} // right triangle:
					i0 = i;
					j0 = j;
					i1 = i + 1;
					j1 = j;
					i2 = i + 1;
					j2 = j + 1;
					if ((zlevels[k] >= pts[i0, j0].z && zlevels[k] < pts[i1, j1].z ||
					     zlevels[k] < pts[i0, j0].z && zlevels[k] >= pts[i1, j1].z) &&
					    (zlevels[k] >= pts[i1, j1].z && zlevels[k] < pts[i2, j2].z ||
					 zlevels[k] < pts[i1, j1].z && zlevels[k] >= pts[i2, j2].z))
					{
						zratio = (zlevels[k] - pts[i0, j0].z)/(pts[i1, j1].z - pts[i0, j0].z);
						pta[0] =
							new Vector2(pts[i0, j0].x*(1 - zratio) + pts[i1, j1].x*zratio, pts[i0, j0].y);
						zratio = (zlevels[k] - pts[i1, j1].z)/(pts[i2, j2].z - pts[i1, j1].z);
						pta[1] =
							new Vector2(pts[i1, j1].x, pts[i1, j1].y*(1 - zratio) + pts[i2, j2].y*zratio);
						//GL.Vertex3(pta[0].x,pta[0].y,0);
						//GL.Vertex3(pta[1].x,pta[1].y,0);
						
						
						
						lineVertexes.Add(new Vector3(pta[0].x,pta[0].y,0));
						lineVertexes.Add(new Vector3(pta[1].x,pta[1].y,0));
						//g.DrawLine(aPen, pta[0], pta[1]);
					}
					else if ((zlevels[k] >= pts[i0, j0].z && zlevels[k] < pts[i2, j2].z ||
					          zlevels[k] < pts[i0, j0].z && zlevels[k] >= pts[i2, j2].z) &&
					         (zlevels[k] >= pts[i1, j1].z && zlevels[k] < pts[i2, j2].z ||
					 zlevels[k] < pts[i1, j1].z && zlevels[k] >= pts[i2, j2].z))
					{
						zratio = (zlevels[k] - pts[i0, j0].z)/(pts[i2, j2].z - pts[i0, j0].z);
						pta[0] =
							new Vector2(pts[i0, j0].x*(1 - zratio) + pts[i2, j2].x*zratio,
							            pts[i0, j0].y*(1 - zratio) + pts[i2, j2].y*zratio);
						zratio = (zlevels[k] - pts[i1, j1].z)/(pts[i2, j2].z - pts[i1, j1].z);
						pta[1] =
							new Vector2(pts[i1, j1].x, pts[i1, j1].y*(1 - zratio) + pts[i2, j2].y*zratio);
						//GL.Vertex3(pta[0].x,pta[0].y,0);
						//GL.Vertex3(pta[1].x,pta[1].y,0);
						
						lineVertexes.Add(new Vector3(pta[0].x,pta[0].y,0));
						lineVertexes.Add(new Vector3(pta[1].x,pta[1].y,0));
						
						//g.DrawLine(aPen, pta[0], pta[1]);
					}
					else if ((zlevels[k] >= pts[i0, j0].z && zlevels[k] < pts[i1, j1].z ||
					          zlevels[k] < pts[i0, j0].z && zlevels[k] >= pts[i1, j1].z) &&
					         (zlevels[k] >= pts[i0, j0].z && zlevels[k] < pts[i2, j2].z ||
					 zlevels[k] < pts[i0, j0].z && zlevels[k] >= pts[i2, j2].z))
					{
						zratio = (zlevels[k] - pts[i0, j0].z)/(pts[i1, j1].z - pts[i0, j0].z);
						pta[0] =
							new Vector2(pts[i0, j0].x*(1 - zratio) + pts[i1, j1].x*zratio, pts[i0, j0].y);
						zratio = (zlevels[k] - pts[i0, j0].z)/(pts[i2, j2].z - pts[i0, j0].z);
						pta[1] =
							new Vector2(pts[i0, j0].x*(1 - zratio) + pts[i2, j2].x*zratio,
							            pts[i0, j0].y*(1 - zratio) + pts[i2, j2].y*zratio);
						lineVertexes.Add(new Vector3(pta[0].x,pta[0].y,0));
						lineVertexes.Add(new Vector3(pta[1].x,pta[1].y,0));
						//GL.Vertex3(pta[0].x,pta[0].y,0);
						//GL.Vertex3(pta[1].x,pta[1].y,0);
						//g.DrawLine(aPen, pta[0], pta[1]);
					}
				}
			}
		}
		return lineVertexes;
	}
	//http://stackoverflow.com/questions/13286485/how-to-draw-2d-contour-plot-in-c
	private  List<Vector3> DrawContour( Vector3[,] pts, float zmin, float zmax, int ncount)
	{
		var zlevels = new float[ncount];
		for (int i = 0; i < ncount; i++)
		{
			zlevels[i] = zmin + i*(zmax - zmin)/(ncount - 1);
		}

		return DrawContour(pts,zlevels);
	}
}
