using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetDeadFailCondition : IFailCondition, IBuild {

	public event FailConditionHandler Fail;
	public Economy economy;
	
	public SolarSystem solarSystem;
	private IList<Planet> planets = new List<Planet>();
	
	public void Start(){
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

	public void Reset () {

		foreach (Body b in solarSystem.bodies) {
			if(b is Planet){
				planets.Add((Planet)b);
				((Planet)b).ResourceLevelChanged-= HandleResourceLevelChanged;
			}
		}
	}
	
	void HandleResourceLevelChanged (Resource resource, float value, float delta)
	{
		if (delta == 0) {
			return;
		}
		//ask all the planets if they're full of the people resource
		foreach (Planet p in planets) {
			Resource r = p.GetResource(Cargo.People);
			if(r!=null && r.current<=0){
				Fail();
			}
		}
		


	}

}
