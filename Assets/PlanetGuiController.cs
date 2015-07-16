using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlanetGuiController : MonoBehaviour {

	public Planet planet;
	private Economy economy;
	public Text price;

	// Use this for initialization
	void Start () {
		economy = FindObjectOfType<Economy> ();
		Resource f = planet.GetResource (Cargo.Food);
		if (f == null) {
			this.gameObject.SetActive(false);
		} else {
			f.ResourceLevelChanged += HandleResourceLevelChanged;
		}
	}

	void HandleResourceLevelChanged (Resource resource, float value, float delta)
	{
		price.text = Mathf.RoundToInt(economy.GetPrice (resource)).ToString ();
	}

}
