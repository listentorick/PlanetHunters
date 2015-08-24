using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CometController : MonoBehaviour, IReset, IBuild, IStartStop {

	public Timer timer;
	public Comet cometPrefab;
	public SolarSystem sol;
	public Pool pool;
	public ShipSpawner spawner;
	public bool stop = true;
	private List<IStartStop> stoppables = new List<IStartStop>();

		
	public void Build(Ready ready) {
		timer.TimerEvent+= HandleTimerEvent;

		pool.PopulatePool (delegate() {
			Body comet = (Body)Instantiate (cometPrefab);
			return comet.gameObject;
		});

		ready ();

	}

	public void Reset(){
		stoppables.Clear ();
		timer.TimerEvent-= HandleTimerEvent;
		pool.Reset ();
		stop = true;
	}

	public void StartPlay(){
		stop = false;
		foreach (IStartStop s in stoppables) {
			s.StartPlay();
		}
	}

	public void StopPlay()
	{
		stop = true;
		foreach (IStartStop s in stoppables) {
			s.StopPlay();
		}
	}

	void HandleTimerEvent ()
	{
		if (stop)
			return;

		GameObject g = pool.GetPooledObject ();
		if (g!=null) {
			Comet c = g.GetComponent<Comet> ();
			spawner.Spawn (c);
			//stoppables.Add(c);

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
