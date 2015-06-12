using UnityEngine;
using System.Collections;

public class WarpGate : MonoBehaviour {


	public delegate void ShipEnteredWarpGateHandler(Ship ship, WarpGate warpGate);
	public event ShipEnteredWarpGateHandler ShipEnteredWarpGate;
	//public Cargo cargoType;
	public Resource resource;



	// Use this for initialization
	void Start () {
	

	}

	public void SetResource(Resource r) {
		resource = r;
		this.GetComponent<SpriteRenderer> ().color = Helpers.GetCargoColor (resource.resourceType);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter2D(Collider2D other) {
		Ship ship = other.gameObject.GetComponent<Ship>();
		if(ship!=null) ShipEnteredWarpGate(ship, this);
	}
	
}
