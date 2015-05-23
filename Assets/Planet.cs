using UnityEngine;
using System.Collections;

public class Planet : Body {

	public float medicalSupplies;
	public float maxMedicalSupplies;
	public float foodSupplies;
	public float maxFoodSupplies;
	public float technologySupplies;

	public float rateOfConsumptionMedicalSupplies;
	public float rateOfConsumptionFoodlSupplies;
	public float rateOfConsumptionTechnologySupplies;

	public ResourceChart medicalSuppliesChart;
	public ResourceChart foodSuppliesChart;
	public ResourceChart technologySuppliesChart;

	private float medicalSupplyTimer = 0;
	private float foodSupplyTimer = 0;
	private Bounds bounds;

	void Start() {
		bounds = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.bounds;
		if (!canMove) {
			float xSize = bounds.size.x;
			this.transform.GetChild (0).transform.localScale = new Vector3 (2 / xSize, 2 / xSize, 1) * soi / scale;   
		}
	}

	void Update () {


		medicalSupplyTimer += Time.deltaTime;
		if (medicalSupplyTimer > rateOfConsumptionMedicalSupplies) {
			medicalSupplyTimer = 0;
			medicalSupplies-=1;

		}

		medicalSuppliesChart.Set(medicalSupplies/maxMedicalSupplies);


		foodSupplyTimer += Time.deltaTime;
		if (foodSupplyTimer > rateOfConsumptionFoodlSupplies) {
			foodSupplyTimer = 0;
			foodSupplies-=1;
			
		}
		
		foodSuppliesChart.Set(foodSupplies/maxFoodSupplies);

		base.Update ();

	}

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
