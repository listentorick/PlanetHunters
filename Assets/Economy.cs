using UnityEngine;
using System.Collections;

public class Economy : MonoBehaviour {

	public SolarSystem solarSystem;
	// Use this for initialization
	void Start () {
	
		solarSystem.ShipEnteredOrbit+= HandleShipEnteredOrbit;
	}

	void HandleShipEnteredOrbit (Body s, Body p)
	{
		((Planet)p).ConsumeCargo ((Ship)s);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
