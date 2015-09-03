using UnityEngine;
using System.Collections;

public class ColonyShipController : BodyController {

	public ColonyShip colonyShipPrefab;
	
	public override Body BuildBody(){
		Body colonyShip = (Body)Instantiate (colonyShipPrefab);
		return colonyShip;
	}
	
	public override Body ConfigureBody(Body b){
		((Ship)b).cargoType = Cargo.People;
		((Ship)b).cargo = 10;
		((Ship)b).fuel = 1;
		return b;
	}

}
