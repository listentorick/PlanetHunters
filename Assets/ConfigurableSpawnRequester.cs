using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfigurableSpawnRequester : BaseSpawnRequester,IStartStop {

	private float timePassed = 0;

	private List<SpawnConfiguration> toSpawn = new List<SpawnConfiguration> ();


	public void Add(SpawnConfiguration s){
		toSpawn.Add (s);
	}


	
	public override void Build(Ready r)
	{
		//sort them. most recent first
		toSpawn.Sort (delegate(SpawnConfiguration s1, SpawnConfiguration s2) {
			
			if(s1.When<s2.When)
			{
				return -1;
			} else if(s1.When>s2.When)
			{
				return 1;
			} else {
				return 0;
			}
			
		});
	}
	
	public override void Reset()
	{
		toSpawn.Clear ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (stopped == false) {
			timePassed += Time.deltaTime;

			if(toSpawn.Count>0) {
				if(toSpawn[0].When<timePassed){
					SpawnConfiguration s = toSpawn[0];
					toSpawn.Remove(s);
					OnSpawnRequest(new Vector2(s.Position.X,s.Position.Y),new Vector2(s.Velocity.X,s.Velocity.Y));
				}
			}

		}
	
	}


}
