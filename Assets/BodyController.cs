using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public delegate void BodyBuiltHandler(Body body);

public class BodyController : MonoBehaviour, IReset, IBuild, IStartStop {

	public Body prefab;

	public BaseSpawnRequester spawnRequester;
	public SolarSystem sol;
	public Pool pool;
	//public ShipSpawner spawner;
	public bool stop = true;
	private List<IStartStop> stoppables = new List<IStartStop>();

	public event BodyBuiltHandler BodyBuilt;  
	
	public void Build(Ready ready) {

		if (spawnRequester != null) {
			spawnRequester.SpawnRequest += HandleSpawnRequest; 
			spawnRequester.Build (delegate {

			});
		}

		pool.PopulatePool (delegate() {
			Body b = BuildBody();
			if(BodyBuilt!=null)BodyBuilt(b);
			b.gameObject.transform.position = new Vector2(-100,-100);
		
			stoppables.Add(b);

			return b.gameObject;

		});
		
		ready ();
		
	}

	public void Spawn(Vector2 pos, Vector2 vel) 
	{
		HandleSpawnRequest (pos, vel);
	}

	void HandleSpawnRequest (Vector2 pos, Vector2 vel)
	{
		if (stop)
			return;

		GameObject g = pool.GetPooledObject ();
		if (g!=null) {
			Body c = ConfigureBody(g.GetComponent<Body> ());
			//c.position = pos;

			c.AlignToVector(vel);
			c.position = GameController.SCALE * pos;
			c.lastPosition = c.position - (vel * Time.fixedDeltaTime);
			c.gameObject.transform.position = pos;
			sol.AddBody(c);
			c.gameObject.SetActive(true);
			c.AlignToVector(vel);
			c.StartPlay(); //start it.

		}
	}

	public virtual Body BuildBody () {
		Body body = (Body)Instantiate (prefab);
		return body;
	}
	

	//public virtual Body ConfigureBody(Body b){
//		return b;
//	}
	
	public void Reset(){
		stoppables.Clear ();
		if (spawnRequester != null) {
			spawnRequester.SpawnRequest -= HandleSpawnRequest; 
			spawnRequester.Reset ();
		}
		pool.Reset ();
		stop = true;
	}
	
	public void StartPlay(){
		stop = false;
		if(spawnRequester!=null) spawnRequester.StartPlay ();
		foreach (IStartStop s in stoppables) {
			s.StartPlay();
		}
	}
	
	public void StopPlay()
	{
		stop = true;
		if(spawnRequester!=null) spawnRequester.StopPlay ();
		foreach (IStartStop s in stoppables) {
			s.StopPlay();
		}
	}

	//public abstract Body ConfigureBody (Body b);

	public virtual Body ConfigureBody(Body b){
		return b;
	}

	public void SpawnNow(){
	
	//	GameObject g = pool.GetPooledObject ();
	//	if (g!=null) {
	//		Body c = ConfigureBody(g.GetComponent<Body> ());
			//spawner.Spawn (c);	
			//should add to solar system here rather than in spawner?
	//	}
	}

	public void ReturnToPool(Body b) {
		b.gameObject.transform.position = new Vector2(100f,100f);
		sol.RemoveConnectionsForBody (b);
		sol.RemoveBody (b);
		b.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
