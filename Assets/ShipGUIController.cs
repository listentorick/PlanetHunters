using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ShipGUIController : MonoBehaviour {

	public Ship ship;
	public Slider slider;
	public SpriteRenderer selected;
	public Image fill;
	public Slider cargo;
	public Image cargoFill;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		selected.enabled = ship.IsSelected;


		cargoFill.color = Helpers.GetCargoColor (ship.cargoType);

			
		cargo.value = (float)ship.cargo / (float)ship.maxCargo;

		if (ship.fuel == 0) {
			fill.color = Color.red;
			slider.value = 1;
		} else {
			fill.color = Color.green;
			slider.value = ship.fuel;
		}
	}
}
