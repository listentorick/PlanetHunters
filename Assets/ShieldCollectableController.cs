using UnityEngine;
using System.Collections;

public class ShieldCollectableController : BaseCollectableController {

	public Timer timer;
	//public CollectablesController controller;
	public void Start(){
		timer.TimerEvent += HandleTimerEvent;
	}

	void HandleTimerEvent ()
	{
		Collectable c = (Collectable)Instantiate (collectablePrefab);
		c.type = CollectableType.Shield;
		c.Collected += HandleCollected;
		OnSpawnRequest (c);
	}

	void HandleCollected (Collectable collectable, Ship ship)
	{
		//what should this do?
		ship.hull = 1f;

	}
}
