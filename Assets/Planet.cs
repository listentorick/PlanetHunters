using UnityEngine;
using System.Collections;

public class Planet : Body {
	
	public soiChart soiChart;
	public ResourceChart resourceChartPrefab;
	private Bounds bounds;

	private Resource[] resourceComponents;
	private ResourceChart[] resourceCharts;

	public Resource[] GetResources(){
		return resourceComponents;
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
			r.ResourceLevelChanged += HandleResourceLevelChanged;
			
			ResourceChart rC = (ResourceChart)Instantiate(resourceChartPrefab);
			rC.transform.parent = this.transform;
			rC.transform.localPosition = new Vector3(0,0,-6f);
			rC.resourceType = r.resourceType;
			resourceCharts[i] =rC;

			rC.Set(r.current);
			
			rC.minAngle = minAngle;
			rC.maxAngle = rC.minAngle + deltaAngle;
			minAngle+=deltaAngle;
			
			
			
		}
	
	}

	void HandleResourceLevelChanged (Cargo type, float value)
	{
		//find the first resourceChart that supports this type...

		for (int i=0; i< resourceCharts.Length; i++) {
			if(resourceCharts[i].resourceType==type) {
				resourceCharts[i].Set(value);
				return;
			}
		}

	}

	void Update () {

		/*
		//foodPriceChart.Set (1);
		//medicalPriceChart.Set (1);
		medicalSupplyTimer += Time.deltaTime;
		if (medicalSupplyTimer > rateOfConsumptionMedicalSupplies) {
			medicalSupplyTimer = 0;
			medicalSupplies-=1;
			if(medicalSupplies<=0){
				ResourceDepleted(Cargo.Medical);
				medicalSupplies = 0;
			}

		}

		medicalSuppliesChart.Set(medicalSupplies/maxMedicalSupplies);


		foodSupplyTimer += Time.deltaTime;
		if (foodSupplyTimer > rateOfConsumptionFoodlSupplies) {
			foodSupplyTimer = 0;
			foodSupplies-=1;
			if(foodSupplies<=0){
				ResourceDepleted(Cargo.Food);
				foodSupplies = 0;
			}
			
		}
		
		foodSuppliesChart.Set(foodSupplies/maxFoodSupplies);


		technologySupplyTimer += Time.deltaTime;
		if (technologySupplyTimer > rateOfConsumptionTechnologySupplies) {
			technologySupplyTimer = 0;
			technologySupplies-=1;
			if(technologySupplies<=0){
				ResourceDepleted(Cargo.Technology);
				technologySupplies = 0;
			}
			
		}
		
		technologySuppliesChart.Set(technologySupplies/maxTechnologySupplies);

*/

		base.Update ();

	}

	public delegate void ResourceDepletedHandler(Cargo type);
	public event ResourceDepletedHandler ResourceDepleted;


	public void ConsumeCargo (Ship s) {
	
		//whats the ship got?
		if (s.cargoType == Cargo.Food) {
			//foodSupplies+= s.cargo;
			//if(foodSupplies>maxFoodSupplies){
		//		foodSupplies = maxFoodSupplies;
		//	}
			s.cargo = 0;
		}
	
	
	
	}


}
