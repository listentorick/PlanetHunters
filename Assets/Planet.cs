using UnityEngine;
using System.Collections;

public class Planet : Body {
	
	public soiChart soiChart;
	public ResourceChart resourceChartPrefab;
	private Bounds bounds;

	private Resource[] resourceComponents;
	private ResourceChart[] resourceCharts;

	public int population = 0;

	public Resource[] GetResources(){
		return resourceComponents;
	}

	public void AddPopulation(int population) {
		this.population += population;
		SetPopulation (this.population);
	}

	private void SetPopulation(int population) {
	
		//for each resource update the rate of consumption
		foreach (Resource r in resourceComponents) {
			//takes 1 second 1 unit
			r.timeToConsumeOneUnit = 10f / population; 
		}
	}

	void Start() {
		//bounds = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.bounds;
		//if (!canMove) {
			float xSize = bounds.size.x;
		//	this.transform.GetChild (0).transform.localScale = new Vector3 (2 / xSize, 2 / xSize, 1) * soi / scale;   
		//}
		soiChart.SetPlanet (this);


	}

	public void BuildResourceCharts() {

		resourceComponents = this.GetComponents<Resource> ();
		
		
		//we need to add a chart for each resource...
		//how many are there?
		resourceCharts = new ResourceChart[resourceComponents.Length ];
		float minAngle = 0;
		float deltaAngle = 360 / resourceComponents.Length;
		for(int i=0; i<resourceComponents.Length;i++) { 
			Resource r = resourceComponents[i];

			ResourceChart rC = (ResourceChart)Instantiate(resourceChartPrefab);
			rC.transform.parent = this.transform;
			rC.transform.localPosition = new Vector3(0,0,-6f);
			rC.resourceType = r.resourceType;
			resourceCharts[i] =rC;

			rC.Set(r);
			
			rC.minAngle = minAngle;
			rC.maxAngle = rC.minAngle + deltaAngle;
			minAngle+=deltaAngle;
			
			
			
		}
	
	}

	void HandleResourceLevelChanged (Cargo type, float value)
	{


	}

	void Update () {
		base.Update ();

	}

	public delegate void ResourceDepletedHandler(Cargo type);
	public event ResourceDepletedHandler ResourceDepleted;


	public void ConsumeCargo (Ship s) {
	
		//whats the ship got?
		//if (s.cargoType == Cargo.Food) {
			//foodSupplies+= s.cargo;
			//if(foodSupplies>maxFoodSupplies){
		//		foodSupplies = maxFoodSupplies;
		//	}
		//	s.cargo = 0;
		//}
	
	
	
	}


}
