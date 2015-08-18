using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CollectableType {	
	Shield,
	Power
}

public class Collectable : Body {

	public CollectableType type;
	public Sprite[] sprites;

	public void Start(){
		for (int i=0; i<sprites.Length; i++) {
			if(sprites[i].name.ToLower() == type.ToString().ToLower()){
				this.GetRendererTransform ().GetComponent<SpriteRenderer> ().sprite = sprites[i];
				break;
			}
		}
	}
	
	public void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "selection") { //this collision is coming from the child
			return;
		}
		TraderShip ship = other.gameObject.GetComponent<TraderShip>();
		if (ship != null) {
			Collected(this,ship);
		}
	}

	public delegate void CollectedHandler(Collectable collectable, Ship ship);
	public event CollectedHandler Collected;

}
