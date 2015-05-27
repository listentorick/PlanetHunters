using UnityEngine;
using System.Collections;

public class Planet : Body {

	public float medicalSupplies;
	public float maxMedicalSupplies;
	public float foodSupplies;
	public float maxFoodSupplies;
	public float technologySupplies;
	public float maxTechnologySupplies;

	public soiChart soiChart;

	public float rateOfConsumptionMedicalSupplies;
	public float rateOfConsumptionFoodlSupplies;
	public float rateOfConsumptionTechnologySupplies;

	public ResourceChart medicalSuppliesChart;
	//public ResourceChart medicalPriceChart;
	public ResourceChart foodSuppliesChart;
	//public ResourceChart foodPriceChart;
	public ResourceChart technologySuppliesChart;

	private float medicalSupplyTimer = 0;
	private float foodSupplyTimer = 0;
	private float technologySupplyTimer = 0;
	private Bounds bounds;

	void Start() {
		//bounds = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.bounds;
		//if (!canMove) {
			float xSize = bounds.size.x;
		//	this.transform.GetChild (0).transform.localScale = new Vector3 (2 / xSize, 2 / xSize, 1) * soi / scale;   
		//}
		soiChart.SetPlanet (this);
	}

	void Update () {
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



		base.Update ();

	}

	public delegate void ResourceDepletedHandler(Cargo type);
	public event ResourceDepletedHandler ResourceDepleted;


	public void ConsumeCargo (Ship s) {
	
		//whats the ship got?
		if (s.cargoType == Cargo.Food) {
			foodSupplies+= s.cargo;
			if(foodSupplies>maxFoodSupplies){
				foodSupplies = maxFoodSupplies;
			}
			s.cargo = 0f;
		}
	
	
	
	}


}
