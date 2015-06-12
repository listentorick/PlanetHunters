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

	public Slider hull;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		selected.enabled = ship.IsSelected;

		if (ship is TraderShip) {
			cargoFill.color = Helpers.GetCargoColor (((TraderShip)ship).cargoType);
			cargo.value = (float)((TraderShip)ship).cargo / (float)((TraderShip)ship).maxCargo;
		
			if (ship.hull == 0) {
				fill.color = Color.red;
				hull.value = 1;
			} else {
				fill.color = Color.green;
				hull.value = ship.hull;
			}

		}

		if (ship.fuel == 0) {
			fill.color = Color.red;
			slider.value = 1;
		} else {
			fill.color = Color.green;
			slider.value = ship.fuel;
		}
	}
}
