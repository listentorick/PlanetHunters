using UnityEngine;
using System.Collections;

public class WarpGate : MonoBehaviour {


	public delegate void ShipEnteredWarpGateHandler(Ship ship);
	public event ShipEnteredWarpGateHandler ShipEnteredWarpGate;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter2D(Collider2D other) {
		Ship ship = other.gameObject.GetComponent<Ship>();
		if(ship!=null) ShipEnteredWarpGate(ship);
	}
	
}
