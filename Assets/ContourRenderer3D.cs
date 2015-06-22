using UnityEngine;
using System.Collections;
using System;

public class ContourRenderer3D : ContourRenderer {

	public GravityFieldHelper fieldHelper;
	public Terrain terrain;

	// Use this for initialization
	public override void Build () {

		Vector3[,] points = fieldHelper.CalculatePoints ();

		MinMax minMax = GetMinMax (points);

		TerrainData tData = terrain.terrainData;

		//tData.heightmapResolution = 513;
		tData.heightmapResolution = points.GetLength(0) + 1;
		//tData.baseMapResolution = 1024;
		//tData.SetDetailResolution(1024, 16);

		tData.size = new Vector3(points.GetLength(0),100, points.GetLength(1));
		//tData.size = new Vector3(points.GetLength(0),100000, points.GetLength(1));


		float[,] heightMap = new float[points.GetLength(0), points.GetLength(1)];

		//float[,] heightMap = new float[tData.heightmapWidth,tData.heightmapHeight];
		


		for (int x = 0; x < points.GetLength(0) ; x++) {
			for (int y = 0; y < points.GetLength(1); y++) {
				//if(points[x,y].z <500000) {
					//heightMap[x,y] = (500000 - points[x,y].z)/5000;
					//heightMap[x,y] = (500000 - points[x,y].z)/10000;
				//	heightMap[x,y] = (500000 - points[x,y].z)/100000;
				//}

				heightMap[x,y] = points[x,y].z/100000;
			}
		}



		tData.SetHeights(0,0,heightMap);
		Debug.Log ("ERP" + points.GetLength (0));
		Debug.Log ("YO" + tData.heightmapHeight + " " + tData.heightmapWidth);
		//Terrain.CreateTerrainGameObject(tData);



	}

	private  MinMax GetMinMax(Vector3[,] points){
		float min = 500000000000000f;
		float max = 0f;
		float current = 0f;
		for (int x = 0; x < points.GetLength(0); x++) {
			for (int y = 0; y < points.GetLength(1); y++) {
				current = points[x,y].z;
				if(current <min) {
					min = current;
				}
				if(current >max) {
					max = current;
				}
			}
		}
		return new MinMax(min,max);
	}
	
	
}

public struct MinMax {
	public MinMax(float min, float max) {
		this.max = max;
		this.min = min;
	}
	public float min;
	public float max;
}
