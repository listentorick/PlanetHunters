using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CometController : MonoBehaviour, IReset, IBuild, IStop {

	public Timer timer;
	public Comet cometPrefab;
	public SolarSystem sol;
	public Pool pool;
	public ShipSpawner spawner;
		
	public void Build(Ready ready) {
		timer.TimerEvent+= HandleTimerEvent;

		pool.PopulatePool (delegate() {
			Body comet = (Body)Instantiate (cometPrefab);
			return comet.gameObject;
		});

		ready ();

	}

	public void Reset(){
		timer.TimerEvent-= HandleTimerEvent;
		pool.Reset ();
	}

	public void Stop()
	{
	
	}

	void HandleTimerEvent ()
	{
		GameObject g = pool.GetPooledObject ();
		if (g!=null) {
			spawner.Spawn (g.GetComponent<Comet> ());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
