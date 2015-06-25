using UnityEngine;
using System.Collections;

public class ShieldCollectableController : BaseCollectableController {

	public Timer timer;
	public Pool pool;
	public SolarSystem solarSystem;

	//public CollectablesController controller;
	public void Start(){
		timer.TimerEvent += HandleTimerEvent;

	}

	bool ShieldNeeded() {
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
	void HandleTimerEvent ()
	{
		if (ShieldNeeded ()) {
			GameObject g = pool.GetPooledObject ();
			if (g!=null) {
				OnSpawnRequest (g.GetComponent<Collectable> ());
			}
		}

	}

	void HandleCollected (Collectable collectable, Ship ship)
	{
		//what should this do?
		ship.hull = 1f;
		collectable.gameObject.SetActive (false);

	}

	public override void Reset()
	{
		pool.Reset ();
	}

	public override void Build()
	{
		pool.PopulatePool (delegate() {
			Collectable c = (Collectable)Instantiate (collectablePrefab);
			c.type = CollectableType.Shield;
			c.Collected += HandleCollected;
			return c.gameObject;
		});
	}


}
