using UnityEngine;
using System.Collections;

public class ShipSpawner : MonoBehaviour {

	public Ship shipPrefab;
	public SolarSystem solarSystem;
	public Planet planetPrefab;
	public Sprite gasPlanetSprite;
	public Sprite bluePlanetSprite;
	public Sprite redPlanetSprite;

	private float elaspedTime = 0;
	private float nextTime = 0;



	// Use this for initialization
	void Start () {

		//position 3 planets ramdomly



		//Gas Giant
		Planet gasGiant = Instantiate (planetPrefab);
		gasGiant.position = new Vector2 (-100000f, -100000f);
		gasGiant.mass = 1e+20f;
		gasGiant.GetComponent<SpriteRenderer> ().sprite = gasPlanetSprite;
		gasGiant.medicalSupplies = 100;
		gasGiant.maxMedicalSupplies = 100;
		gasGiant.rateOfConsumptionMedicalSupplies = 10;
		gasGiant.soi = 100000;
		gasGiant.canMove = false;

		solarSystem.AddBody (gasGiant);


		Planet redPlanet = Instantiate (planetPrefab);
		redPlanet.position = new Vector2 (200000f, 40000);
		redPlanet.mass = 1e+20f;
		redPlanet.GetComponent<SpriteRenderer> ().sprite = redPlanetSprite;
		redPlanet.medicalSupplies = 100;
		redPlanet.maxMedicalSupplies = 100;
		redPlanet.rateOfConsumptionMedicalSupplies = 10;
		redPlanet.soi = 100000;
		redPlanet.canMove = false;

		solarSystem.AddBody (redPlanet);


		Planet bluePlanet = Instantiate (planetPrefab);
		bluePlanet.position = new Vector2 (200000, 500000);
		bluePlanet.mass = 1e+24f;
		bluePlanet.GetComponent<SpriteRenderer> ().sprite = bluePlanetSprite;
		bluePlanet.medicalSupplies = 100;
		bluePlanet.maxMedicalSupplies = 100;
		bluePlanet.rateOfConsumptionMedicalSupplies = 10;
		bluePlanet.soi = 150000;
		bluePlanet.canMove = false;

		solarSystem.AddBody (bluePlanet);



	}

	
	//planet will add itself
	public void AddPlanet(Planet planet) {
		
		//each planet will have 3 resources it needs
		//medial supplies 
		//food
		//technology

		//we spawn ships with cargo to ensure that there is
		//always sufficient shizzle
		
		
		
	}
	
	// Update is called once per frame
	void Update () {

		if (elaspedTime > nextTime) {
			nextTime = Random.Range(10,30);
			elaspedTime = 0f;
			Ship newShip = Instantiate(shipPrefab);
			newShip.position = new Vector2(Random.Range(-10000,10000),Random.Range(-10000,10000));
			newShip.cargoType = Cargo.Food;
			newShip.cargo = 10f;
			solarSystem.AddBody(newShip);
		}
		elaspedTime += Time.deltaTime;
	}
}
