using UnityEngine;
using System.Collections;

public class ShieldCollectableController : BaseCollectableController {

	public Timer timer;
	public Pool pool;

	//public CollectablesController controller;
	public void Start(){
		timer.TimerEvent += HandleTimerEvent;
		pool.PopulatePool (delegate() {
			Collectable c = (Collectable)Instantiate (collectablePrefab);
			c.type = CollectableType.Shield;
			c.Collected += HandleCollected;
			return c.gameObject;
		});
	}

	void HandleTimerEvent ()
	{
		GameObject g = pool.GetPooledObject ();
		if (g!=null) {
			OnSpawnRequest (g.GetComponent<Collectable> ());
		}
	}

	void HandleCollected (Collectable collectable, Ship ship)
	{
		//what should this do?
		ship.hull = 1f;
		collectable.gameObject.SetActive (false);

	}
}
