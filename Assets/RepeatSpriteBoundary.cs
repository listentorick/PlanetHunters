
	using UnityEngine;
	using System.Collections;
using System.Collections.Generic;


	// @NOTE the attached sprite's position should be "top left" or the children will not align properly
	// Strech out the image as you need in the sprite render, the following script will auto-correct it when rendered in the game
	[RequireComponent (typeof (SpriteRenderer))]
	
	// Generates a nice set of repeated sprites inside a streched sprite renderer
	// @NOTE Vertical only, you can easily expand this to horizontal with a little tweaking
	public class RepeatSpriteBoundary : MonoBehaviour, IBuild, IReset {
		SpriteRenderer sprite;

		public float gridX = 0.0f;
		public float gridY = 0.0f;

		private List<GameObject> sprites = new List<GameObject>();

	public void Build(Ready r){

		float worldScreenHeight = (float)(Camera.main.orthographicSize * 2.0);
		float worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width);
		
		sprite = GetComponent<SpriteRenderer>();
		
		sprite.transform.position = new Vector3(worldScreenWidth/2f, (worldScreenHeight/2f),1f);
		
		Vector2 spriteSize_wu = new Vector2(sprite.bounds.size.x / transform.localScale.x, sprite.bounds.size.y / transform.localScale.y);
		Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);
		
		gridX = worldScreenWidth / spriteSize_wu.x;
		gridY = worldScreenWidth / spriteSize_wu.y;
		
		
		if (0.0f != gridX) {
			float width_wu = sprite.bounds.size.x / gridX;
			scale.x = width_wu / spriteSize_wu.x;
			spriteSize_wu.x = width_wu;
		}
		
		if (0.0f != gridY) {
			float height_wu = sprite.bounds.size.y / gridY;
			scale.y = height_wu / spriteSize_wu.y;
			spriteSize_wu.y = height_wu;
		}
		
		GameObject childPrefab = new GameObject();
		
		SpriteRenderer childSprite = childPrefab.AddComponent<SpriteRenderer>();
		childPrefab.transform.position = transform.position;
		childSprite.sprite = sprite.sprite;
		
		GameObject child;
		for (int i = 0, h = (int)Mathf.Round(sprite.bounds.size.y); i*spriteSize_wu.y < h; i++) {
			for (int j = 0, w = (int)Mathf.Round(sprite.bounds.size.x); j*spriteSize_wu.x < w; j++) {
				child = Instantiate(childPrefab) as GameObject;
				child.transform.position = transform.position - (new Vector3(spriteSize_wu.x*j, spriteSize_wu.y*i, 0));
				child.transform.localScale = scale;
				child.transform.parent = transform;
				sprites.Add(child);
			}
		}
		
		
		
		float width = sprite.sprite.bounds.size.x;
		float height = sprite.sprite.bounds.size.y;
		
		
		float scaleX = worldScreenWidth / width;
		float scaleY = worldScreenHeight / height;
		
		
		float scaleToUse = 1;
		if (scaleX > scaleY) {
			scaleToUse = scaleX;
		} else {
			scaleToUse = scaleY;
		}
		
		this.transform.localScale = new Vector2(scaleToUse, scaleToUse);
		
		Destroy(childPrefab);
		sprite.enabled = false; // Disable this SpriteRenderer and let the prefab children render themselves
		r ();
	}
 	
	public void Reset() {
		foreach (GameObject g in sprites) {
			Destroy (g);
		}
		sprites.Clear ();
		if(sprite!=null) sprite.enabled = true;
		this.transform.localScale = new Vector3 (1, 1, 1);
	}

	void Start () {

		


	}

}
