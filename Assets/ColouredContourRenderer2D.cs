using UnityEngine;
using System.Collections;

public class ColouredContourRenderer2D : ContourRenderer {

	public GravityFieldHelper fieldHelper;
	public Terrain terrain;


	void ResizeSpriteToScreen(GameObject theSprite, Camera theCamera, int fitToScreenWidth, int fitToScreenHeight)
	{        
		SpriteRenderer sr = theSprite.GetComponent<SpriteRenderer>();
		
		theSprite.transform.localScale = new Vector3(1,1,1);
		
		float width = sr.sprite.bounds.size.x;
		float height = sr.sprite.bounds.size.y;
		
		float worldScreenHeight = (float)(theCamera.orthographicSize * 2.0);
		float worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width);

		float scaleX = worldScreenWidth / width;
		float scaleY = worldScreenHeight / height;


		float scaleToUse = 1;
		if (scaleX > scaleY) {
			scaleToUse = scaleX;
		} else {
			scaleToUse = scaleY;
		}
		//if (fitToScreenWidth != 0)
		//{
			//Vector2 sizeX = new Vector2(worldScreenWidth / width / fitToScreenWidth,worldScreenHeight / height / fitToScreenHeight);
			//theSprite.transform.localScale = sizeX;
		//}
		
		//if (fitToScreenHeight != 0)
	//	{
		Vector2 scale = new Vector2(scaleToUse, scaleToUse);
			theSprite.transform.localScale = scale;
	//	}
	}


	public void Start() {


	//	var cam = Camera.main;
	//	Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane));  
	//	Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane));

	//	transform.localScale = new Vector3(topRight.x - bottomLeft.x,topRight.y-bottomLeft.y,cam.nearClipPlane);


		//v3BottomLeft = Camera.main.ViewportToWorldPoint(v3ViewPort);
		//v3ViewPort.Set(1,1,distance);
		//v3TopRight = Camera.main.ViewportToWorldPoint(v3ViewPort);
		
		//transform.localScale = new Vector3(v3BottomLeft.x-v3TopRight.x,v3BottomLeft.y-v3TopRight.y,goDepth);
		
	}
	
	// Use this for initialization
	public override void Build () {
		
		Vector3[,] points = fieldHelper.CalculatePoints (Screen.width,Screen.height);
		float[,] heightMap = new float[points.GetLength(0), points.GetLength(1)];
		for (int x = 0; x < points.GetLength(0) ; x++) {
			for (int y = 0; y < points.GetLength(1); y++) {
				if(points[x,y].z <10000) {
					heightMap[x,y] = points[x,y].z/10000;
				} else {
					heightMap[x,y] = 1;
				}
			}
		}





		Texture2D texture = HeightMapToPNG (heightMap, 1);

		SpriteRenderer r = this.GetComponent<SpriteRenderer>();

		r.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));

		ResizeSpriteToScreen (this.gameObject, Camera.main, 1, 0);
		Debug.Log ("center " + r.bounds.center);
		//MaterialPropertyBlock block = new MaterialPropertyBlock();
		//block.AddTexture("_MainTex",texture);
		//r.SetPropertyBlock(block);

		Vector3 pivotOffset = transform.position - r.bounds.center;
	//	r.bounds.size
		this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x + pivotOffset.x, this.gameObject.transform.position.y + pivotOffset.y,5f);
		
	}
	
	private Color GetColor(float value) {

		float alpha = 0;

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
		//return new Color (1f,1f,0f, alpha);

		return new Color (52f/255f,97f/255f,99f/255f, alpha);


	}

	private Texture2D RenderBlock(Texture2D texture, float[,] points, int startX, int startY, int width) {
	
		//for(int x = 0; x <width; x++ ){

		//	for( int y=0; y<width;y++) {
				float value = points[startX ,startY ];
				Color c = GetColor(value);
		texture.SetPixel (startX, startY, c);
		//	}
	//	}

		return texture;
	
	}
	
	private Texture2D HeightMapToPNG(float[,] points, int delta) {

		var duplicateHeightMap = new Texture2D(points.GetLength(0), points.GetLength(1), TextureFormat.ARGB32, false);
		//float[,] rawHeights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
		
		/// run through the array row by row
		for (int y=0; y < duplicateHeightMap.height; ++y)
		{
			for (int x=0; x < duplicateHeightMap.width; ++x)
			{
				RenderBlock(duplicateHeightMap, points, x, y, delta);
			}
		}
		duplicateHeightMap.Apply();

		//byte[] myBytes = duplicateHeightMap.EncodeToPNG();

		//System.IO.File.WriteAllBytes(Application.dataPath + "/heightmap.png", myBytes);
		//Debug.Log (Application.dataPath);
		
		return duplicateHeightMap;
	}
	

	
	public override void Reset() {
	}
	
	
}

