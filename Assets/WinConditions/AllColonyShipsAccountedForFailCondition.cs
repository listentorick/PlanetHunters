using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllColonyShipsAccountedForFailCondition : IFailCondition {

	//public GameController gameController;
	public ColonyShipController colonyShipController;

	public event FailConditionHandler Fail;
	private IList<Planet> planets = new List<Planet>();
	public SolarSystem solarSystem;
	
	public void Build (Ready r) {

		planets.Clear ();
		foreach (Body b in solarSystem.bodies) {
			if(b is Planet){
				planets.Add((Planet)b);
			}
		}
		r ();

		colonyShipController.spawnRequester.SpawnRequest+= HandleSpawnRequest;
		
	}

	public void Reset () {
		colonyShipController.spawnRequester.SpawnRequest-= HandleSpawnRequest;
		
	}

	void HandleSpawnRequest (Vector2 position, Vector2 velocity)
	{
		if (colonyShipController.spawnRequester.IsComplete()) {
			
			//are all the planets populated?
			foreach (Planet p in planets) {
				Resource r = p.GetResource(Cargo.People);
				if(r!=null && !r.IsFull()){
					Fail();
				}
			}
			
		}
	}
	

		

}
