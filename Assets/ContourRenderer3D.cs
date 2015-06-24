using UnityEngine;
using System.Collections;
using System;

public class ContourRenderer3D : ContourRenderer {

	public GravityFieldHelper fieldHelper;
	public Terrain terrain;

	// Use this for initialization
	public override void Build () {

		Vector3[,] points = fieldHelper.CalculatePoints (500);

		MinMax minMax = GetMinMax (points);

		TerrainData tData = terrain.terrainData;

		//tData.heightmapResolution = 513;

		//tData.baseMapResolution = 1024;
		//tData.SetDetailResolution(1024, 16);

		//tData.size = new Vector3(points.GetLength(0),100, points.GetLength(1));


		//this is the size of the world.
		//It has nothing to do with the heightmap
		tData.size = new Vector3 (1, 1, 1);

		//tData.size = new Vector3(points.GetLength(0),100000, points.GetLength(1));

		//this is the resolution of the heightmap we're about to add
		tData.heightmapResolution = points.GetLength(0)	 + 1;
		float[,] heightMap = new float[points.GetLength(0), points.GetLength(1)];

		//float[,] heightMap = new float[tData.heightmapWidth,tData.heightmapHeight];
		


		for (int x = 0; x < points.GetLength(0) ; x++) {
			for (int y = 0; y < points.GetLength(1); y++) {
				if(points[x,y].z <500000) {
					heightMap[points.GetLength(0)-1-x,y] = (100000 - points[x,y].z)/100000;
				}

				//heightMap[x,y] = points[x,y].z/100000;
			}
		}



		tData.SetHeights(0,0,heightMap);
		Debug.Log ("ERP" + points.GetLength (0));
		Debug.Log ("YO" + tData.heightmapHeight + " " + tData.heightmapWidth);
		//Terrain.CreateTerrainGameObject(tData);
		HeightMapToPNG ();


	}

	private void HeightMapToPNG() {
		//// get the terrain heights into an array and apply them to a texture2D
		//var myBytes : byte[];
		//var myIndex : int = 0;
		//var rawHeights = new Array(0.0,0.0);
		var duplicateHeightMap = new Texture2D(terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight, TextureFormat.ARGB32, false);
		float[,] rawHeights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
		
		/// run through the array row by row
		for (int y=0; y < duplicateHeightMap.height; ++y)
		{
			for (int x=0; x < duplicateHeightMap.width; ++x)
			{
				/// for wach pixel set RGB to the same so it's gray
				//var color = new Vector4(rawHeights[myIndex], rawHeights[myIndex], rawHeights[myIndex], 1.0);
				duplicateHeightMap.SetPixel (x, y, new Color(rawHeights[x,y],rawHeights[x,y],rawHeights[x,y],1.0f));
				//myIndex++;
			}
		}
		// Apply all SetPixel calls
		duplicateHeightMap.Apply();
		
		/// make it a PNG and save it to the Assets folder
		byte[] myBytes = duplicateHeightMap.EncodeToPNG();
		//var filename : String = "DupeHeightMap.png";
		System.IO.File.WriteAllBytes(Application.dataPath + "/heightmap.png", myBytes);
		Debug.Log (Application.dataPath);
		//EditorUtility.DisplayDialog("Heightmap Duplicated", "Saved as PNG in Assets/ as: " + filename, "");

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

	public override void Reset() {
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
