using UnityEngine;
using System.Collections;

public class TimerSpawnRequester : BaseSpawnRequester {

	public Timer timer;
	public ShipSpawner spawner;

	public override void Build(Ready r){
		timer.TimerEvent += HandleTimerEvent;
		r ();
	}

	public override void Reset() {
		timer.TimerEvent -= HandleTimerEvent;
	}

	void HandleTimerEvent ()
	{
		if (IsNeeded ()) {
			Vector2 pos = new Vector2();
			Vector2 vel = new Vector2();
			spawner.Spawn(ref pos,ref vel);

			OnSpawnRequest (pos,vel);
		}
		
	}

	public virtual bool IsNeeded ()
	{
		return true;
	}

}
