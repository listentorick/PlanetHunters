using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshHelper {

	public void BuildQuad(MeshBuilder meshBuilder, Vector3 offset, Vector3 widthDir, Vector3 lengthDir)
	{
		Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;
	
		meshBuilder.Vertices.Add(offset);
		meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
		meshBuilder.Normals.Add(normal);
	
		meshBuilder.Vertices.Add(offset + lengthDir);
		meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
		meshBuilder.Normals.Add(normal);
	
		meshBuilder.Vertices.Add(offset + lengthDir + widthDir);
		meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
		meshBuilder.Normals.Add(normal);
	
		meshBuilder.Vertices.Add(offset + widthDir);
		meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
		meshBuilder.Normals.Add(normal);
	
		int baseIndex = meshBuilder.Vertices.Count - 4;
	
		meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
		meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
	}

	private List<Vector2> GetCircleEdge(float radius, int numEdges, float startAngle, float endAngle) {
		List<Vector2> points = new List<Vector2> ();
		float deltaAngle = (endAngle-startAngle) / numEdges;
		float angle = startAngle;
		float x;
		float y;
	
		for (int i=0; i<numEdges; i++) {
			y = radius  * Mathf.Cos (angle);
			x = radius * Mathf.Sin (angle);
			Vector2 point = new Vector2 (x, y);
			angle += deltaAngle;
			points.Add(point);
		}

		return points;
	
	}

	public void BuildDisc(MeshBuilder meshBuilder, float innerRadius, float outerRadius, int numEdges, float minAngle, float maxAngle){
	
		List<Vector2> innerCircle = GetCircleEdge (innerRadius, numEdges, minAngle * Mathf.Deg2Rad,maxAngle * Mathf.Deg2Rad);
		List<Vector2> outerCircle = GetCircleEdge (outerRadius, numEdges,minAngle * Mathf.Deg2Rad,maxAngle * Mathf.Deg2Rad);

		Vector3 normal = Vector3.up;

	//	innerCircle.Add (new Vector2 (-0.001f, innerRadius));
		innerCircle.Reverse ();

	//	outerCircle.Add (new Vector2 (-0.001f, outerRadius));
	//	//outerCircle.Add (new Vector2 (0, innerRadius));
		outerCircle.AddRange (innerCircle);

		Triangulator t = new Triangulator ();
		int[] indices = t.Triangulate (outerCircle.ToArray ());
		foreach (Vector2 v in outerCircle) {
			meshBuilder.Vertices.Add(new Vector3(v.x,v.y,0));
		}
		//meshBuilder.Vertices.AddRange (outerCircle);
		for (int i=0; i<indices.Length; i+=3) {
					
			meshBuilder.AddTriangle(indices[i],indices[i+1],indices[i+2]);

		}

		/*
		//Now lets collect the vertices
		int pointIndex = 0;
		int innerIndex = 0;
		int outerIndex = 0;


		for (int i=0; i<(numEdges/2); i++) {

			//For each Edge we have 2 triangles
			//see http://www.siltanen-research.net/meshes.png
			Debug.Log("OI" + i);
			//First Triangle
			Vector3 point1 = outerCircle [outerIndex];
			meshBuilder.Vertices.Add (point1);
			meshBuilder.Normals.Add (normal);

			Vector3 point2 = outerCircle [outerIndex+1];
			meshBuilder.Vertices.Add (point2);
			meshBuilder.Normals.Add (normal);

			Vector3 point3 = innerCircle [innerIndex];
			meshBuilder.Vertices.Add (point3);
			meshBuilder.Normals.Add (normal);
			 
			meshBuilder.AddTriangle(pointIndex,pointIndex+1,pointIndex+2);

			pointIndex+=3;
			innerIndex+=1;
			outerIndex+=2;
		}*/



	
	}
	
}
