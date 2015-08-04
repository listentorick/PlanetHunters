using UnityEngine;
using System.Collections;

public class ShieldCollectableController : BaseTimerCollectableController {
	
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
	
	public override void HandleCollected (Collectable collectable, Ship ship)
	{
		//what should this do?
		ship.hull = 1f;
		collectable.gameObject.SetActive (false);

	}


	public override Collectable BuildCollectable (Collectable c) {

		c.type = CollectableType.Shield;
		return c;
	}



}
