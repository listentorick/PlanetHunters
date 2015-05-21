using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ShipGUIController : MonoBehaviour {

	public Body ship;
	public Slider slider;
	public SpriteRenderer selected;
	public Image fill;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		selected.enabled = ship.IsSelected;
			
	

		if (ship.fuel == 0) {
			fill.color = Color.red;
			slider.value = 1;
		} else {
			fill.color = Color.green;
			slider.value = ship.fuel;
		}
	}
}
