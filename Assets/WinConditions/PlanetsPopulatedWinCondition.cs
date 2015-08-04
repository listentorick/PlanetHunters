using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetsPopulatedWinCondition: IWinCondition, IBuild {

	//public delegate void WinConditionHandler();
	public event WinConditionHandler Win;
	
	public SolarSystem solarSystem;
	private IList<Planet> planets = new List<Planet>();

	public void Start(){
		//solarSystem
	}
	
	public void Build () {
		planets.Clear ();
		foreach (Body b in solarSystem.bodies) {
			if(b is Planet){
				planets.Add((Planet)b);
				((Planet)b).ResourceLevelChanged+= HandleResourceLevelChanged;
			}
		}
	
	}

	void HandleResourceLevelChanged (Resource resource, float value, float delta)
	{
		//ask all the planets if they're full of the people resource
		foreach (Planet p in planets) {
			if(!p.GetResource(Cargo.People).IsFull()){
				return;
			}
		}

		Win ();
	}


}
