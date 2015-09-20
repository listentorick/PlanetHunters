using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllColonyShipsAccountedForFailCondition : IFailCondition {

	public GameController gameController;
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

		solarSystem.ShipEnteredOrbit += HandleShipEnteredOrbit;
		gameController.ShipDestroyed += HandleShipDestroyed;

		r ();
	
	}

	void HandleShipDestroyed ()
	{
		CheckFail ();
	}

	void HandleShipEnteredOrbit (Body s, Body p)
	{
		CheckFail ();
	}

	public void Reset () {
		solarSystem.ShipEnteredOrbit -= HandleShipEnteredOrbit;
		gameController.ShipDestroyed -= HandleShipDestroyed;

	}

	private void CheckFail ()
	{
		//if the solar system contains colony ships we arent complete
		foreach (Body b in solarSystem.bodies) {
			if(b is ColonyShip){
				return;
			}
		}

		//if there arent any more colony ships to spawn
		if (colonyShipController.spawnRequester.IsComplete() ) {
			
			//are all the planets populated?
			foreach (Planet p in planets) {
				Resource r = p.GetResource(Cargo.People);
				if(r!=null && !r.IsFull()){
					Fail("You've run out of colonists!");
				}
			}
			
		}
	}
	

		

}
