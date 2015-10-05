using UnityEngine;
using System.Collections;

public class ShipController : BodyController {

	public Ship colonyShipPrefab;
	public Cargo cargoType;
	public int cargo;
	public float fuel;
	
	public override Body BuildBody(){
		Body colonyShip = (Body)Instantiate (colonyShipPrefab);
		return colonyShip;
	}
	
	public override Body ConfigureBody(Body b){
		((Ship)b).cargoType = cargoType;
		((Ship)b).cargo = cargo;
		((Ship)b).fuel = fuel;
		return b;
	}

}
