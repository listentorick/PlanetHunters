using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class ContourRenderer : MonoBehaviour {

	public SolarSystem sol;
	public LineRenderer lineRenderer;
	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {

		if (lines != null && lines.Count > 0) {
			lineRenderer = this.GetComponent<LineRenderer> ();
			lineRenderer.SetVertexCount (lines.Count);
			for (int i=0; i<lines.Count; i++) {
			
				lineRenderer.SetPosition (i, lines [i]);
			}
		}
		
	}

	public Vector3[,] CalculatePoints() {
	

		var cam = Camera.main;
		Vector3 p1 = cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane));  
		Vector3 p2 = cam.ViewportToWorldPoint(new Vector3(1,0,cam.nearClipPlane));
		Vector3 p3 = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane));
		
		float width = (p2 - p1).magnitude;
		float height = (p3 - p2).magnitude;

		int density =100;
		float delta = width / density;

		Vector3[,] points = new Vector3 [density, density];
		float xpos = 0;
		float ypos = 0;
		for (int x=0; x<density; x++) {

			for(int y=0;y<density;y++){
			
				xpos = (float)x*delta - width/2;
				ypos = (float)y*delta - height/2;

					points[x,y] = new Vector3(xpos,ypos,sol.CalculateForceAtPoint(new Vector2(xpos * 100000f,ypos * 100000f)).magnitude);
				
			
			}
		}
	
		return points;
	
	}


	private Material lineMaterial;

	void CreateLineMaterial() 
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
	public void Build() {
		ready = true;
		gravityPoints = CalculatePoints ();
		lines = DrawContour (gravityPoints, 0, 50000, 20);
	
	}

	private Color mainColor = new Color(0f,1f,0f,0.1f);

	void OnPostRender() {


		return;
		if (!ready) {
			return;
		}

		CreateLineMaterial();
		// set the current material
		lineMaterial.SetPass( 0 );
		
		GL.Begin( GL.LINES );

		GL.Color(mainColor);

		for(int i=0;i<lines.Count;i++){
			GL.Vertex3(lines[i].x,lines[i].y,-8);
			//lineRenderer.SetPosition(i,lines[i]);
		}
		//DrawContour (gravityPoints, 1000, 100000, 80);

		GL.End();
		//ready = false;
	}

	//http://stackoverflow.com/questions/13286485/how-to-draw-2d-contour-plot-in-c
	private  List<Vector3> DrawContour( Vector3[,] pts, float zmin, float zmax, int ncount)
	{

		List<Vector3> lineVertexes = new List<Vector3> ();

		//using (var aPen = new Pen(Color.DimGray) {Width = 0.25f})
		//{
			var pta = new Vector2[2];
			
			
			var zlevels = new float[ncount];
			for (int i = 0; i < ncount; i++)
			{
				zlevels[i] = zmin + i*(zmax - zmin)/(ncount - 1);
			}
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

		//GL.End();
		//}
	}
}
