using UnityEngine;
using System.Collections;

public class ShieldSpawnRequester : TimerSpawnRequester {

	public SolarSystem solarSystem;
	public override bool IsNeeded() {
		foreach (Body b in solarSystem.bodies) {
			if(b is TraderShip){
				//only provide a shield if required....
				if(((TraderShip)b).hull<0.5f){
					return true;
				}
			}
		}
		return false;
	}
}
