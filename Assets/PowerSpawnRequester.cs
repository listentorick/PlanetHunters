using UnityEngine;
using System.Collections;

public class PowerSpawnRequester : TimerSpawnRequester {
	
	public SolarSystem solarSystem;
	public override bool IsNeeded() {
		foreach (Body b in solarSystem.bodies) {
			if(b is TraderShip){
				//only provide a shield if required....
				if(((TraderShip)b).fuel<0.25f){
					return true;
				}
			}
		}
		return false;
	}
}
