using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetsPopulatedWinCondition: IWinCondition, IBuild {

	//public delegate void WinConditionHandler();
	public event WinConditionHandler Win;
	public Economy economy;
	
	public SolarSystem solarSystem;
	private IList<Planet> planets = new List<Planet>();

	public void Start(){
		//solarSystem
	}
	
	public void Build (Ready r) {
		planets.Clear ();
		foreach (Body b in solarSystem.bodies) {
			if(b is Planet){
				planets.Add((Planet)b);
				((Planet)b).ResourceLevelChanged+= HandleResourceLevelChanged;
			}
		}

		r ();
	
	}

	void HandleResourceLevelChanged (Resource resource, float value, float delta)
	{
		//ask all the planets if they're full of the people resource
		foreach (Planet p in planets) {
			Resource r = p.GetResource(Cargo.People);
			if(r!=null && !r.IsFull()){
				return;
			}
		}

		WinData winData = new WinData ();
		winData.Score = (int)economy.playersMoney;
		Win (winData);
	}


}
