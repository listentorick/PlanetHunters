using UnityEngine;
using System.Collections;

public class SpriteResizer : MonoBehaviour {

	//private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start () {

	}
	
	public void ResizeSpriteToScreen(Camera theCamera, int fitToScreenWidth, int fitToScreenHeight)
	{        
		//SpriteRenderer sr = sprite.GetComponent<SpriteRenderer>();
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

		this.transform.localScale = new Vector3(1,1,1);
		
		float width = spriteRenderer.sprite.bounds.size.x;
		float height = spriteRenderer.sprite.bounds.size.y;
		
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
		
		Vector2 scale = new Vector2(scaleToUse, scaleToUse);
		this.transform.localScale = scale;
		
	}
}
