using UnityEngine;
using System.Collections;

public abstract class BaseTimerCollectableController : BaseCollectableController {

	public Timer timer;
	public Pool pool;
	public SolarSystem solarSystem;
	
	//public CollectablesController controller;
	public void Start(){
		timer.TimerEvent += HandleTimerEvent;
		
	}
	
	public abstract bool IsNeeded ();

	void HandleTimerEvent ()
	{
		if (IsNeeded ()) {
			GameObject g = pool.GetPooledObject ();
			if (g!=null) {
				OnSpawnRequest (g.GetComponent<Collectable> ());
			}
		}
		
	}
	
	public abstract void HandleCollected (Collectable collectable, Ship ship);

	
	public override void Reset()
	{
		pool.Reset ();
	}

	public abstract Collectable BuildCollectable (Collectable c);
	
	public override void Build()
	{
		pool.PopulatePool (delegate() {
			Collectable c = (Collectable)Instantiate (collectablePrefab);
			BuildCollectable(c);
			c.Collected += HandleCollected;
			return c.gameObject;
		});
	}
}
