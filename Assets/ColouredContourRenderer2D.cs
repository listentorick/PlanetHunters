using UnityEngine;
using System.Collections;

public class ColouredContourRenderer2D : ContourRenderer {

	public GravityFieldHelper fieldHelper;
	public float scale = 10000;



	public void Start() {

	}

	public void Log(string message){
		System.DateTime now = System.DateTime.Now;
		Debug.Log( string.Format("[{0}] {1}", now.Ticks/100000, message) );
	}

	public float max = 0;
	public float min = float.MaxValue;
	public float range = 0;
	// Use this for initialization
	public override void Build (Ready ready) {
		//return;


		//disable
		//ready ();

		//return;

		fieldHelper.CalculatePoints (Screen.width/40,Screen.height/40, delegate 
			(Vector3[,] points){

			//Log ("Calulate points");
			//Vector3[,] points = fieldHelper.CalculatePoints (Screen.width/2,Screen.height/2); //slow ish
			
			//Log ("End Calculate points");
			
			int numX = points.GetLength (0);
			int numY = points.GetLength (1);
			float[,] heightMap = new float[numX, numY];
			for (int x = 0; x < numX ; x++) {
				for (int y = 0; y < numY; y++) {

					//if(points[x,y].z>max){
					//	max  = points[x,y].z;
					//}

					//if(points[x,y].z<min){
				//		min  = points[x,y].z;
			//		}

					if(points[x,y].z <scale) {
						heightMap[x,y] = points[x,y].z/scale;
					} else {
						heightMap[x,y] = 1;
					}
				}
			}
			//return;


		 	//range = scale-min;
			
			Log ("end building heightmap" );
			
			Texture2D texture = HeightMapToPNG (heightMap, 1);
			
			Log ("end building heightmappng");
			//return;
			
			
			SpriteRenderer r = this.GetComponent<SpriteRenderer>();
			
			r.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));

			Helpers.ResizeSpriteToScreen (this.gameObject, Camera.main, 1, 0);
			Log ("end resize to screen");
			
			
			
			Vector3 pivotOffset = transform.position - r.bounds.center;
			
			this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x + pivotOffset.x, this.gameObject.transform.position.y + pivotOffset.y,5f);
			
			Log ("end ");
			ready();
			
			}
	); //slow ish






	}

	
	private Color GetColor(float value) {

		float alpha = 0;
		//value = value / range;

		if (value < 0.2f) {

			alpha = 0f;

			
		} else if (value < 0.4f) {
			alpha = 0.4f;
		} else if (value < 0.6f) {
			alpha = 0.6f;
		} else if (value < 0.8f) {
			alpha = 0.8f;
		
		} else if (value <= 1f) {
			alpha = 1.0f;
		
		}
		return new Color (52f/255f,97f/255f,99f/255f, alpha);


	}

	private Texture2D RenderBlock(Texture2D texture, float[,] points, int startX, int startY, int width) {
	

		float value = points[startX ,startY ];
		Color c = GetColor(value);
		texture.SetPixel (startX, startY, c);

		return texture;
	
	}
	
	private Texture2D HeightMapToPNG(float[,] points, int delta) {



		var duplicateHeightMap = new Texture2D(points.GetLength(0), points.GetLength(1), TextureFormat.ARGB32, false);

		/// run through the array row by row
		for (int y=0; y < duplicateHeightMap.height; ++y)
		{
			for (int x=0; x < duplicateHeightMap.width; ++x)
			{
				RenderBlock(duplicateHeightMap, points, x, y, delta);
			}
		}
		duplicateHeightMap.Apply();

		return duplicateHeightMap;
	}
	

	
	public override void Reset() {
		this.gameObject.transform.position = new Vector3 ();

	}
	
	
}

