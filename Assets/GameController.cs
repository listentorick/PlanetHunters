﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour, IGameController {

	public Blades bladePrefab;
	public Explosion explosionPrefab;
	public AudioSource audioSource;
	public AudioClip backgroundSound;
	public AudioClip profitSound;
	public AudioClip warpSound;
	public AudioClip failSound;

	public PopularityController popularityController;
	public ShipIndicator shipIndicatorPrefab;
	private IList<Ship> ships = new List<Ship> (); //this is all the ships
	private IList<TraderShip> traderShipPool = new List<TraderShip> ();
	private IList<ColonyShip> colonyShipPool = new List<ColonyShip> ();
	public Material lightMaterial;

	private IList<ShipIndicator> shipIndicators = new List<ShipIndicator> ();
	//private int shipPoolSize = 5;
	public int maxNumShips = 1;

	public TraderShip traderShipPrefab;
	public ColonyShip colonyShipPrefab;

	public Timer colonyShipTimer;
	public Timer traderShipTimer;

	public SolarSystem solarSystem;
	public Planet planetPrefab;
	public Sprite gasPlanetSprite;
	public Sprite bluePlanetSprite;
	public Sprite redPlanetSprite;
	public Sprite sunSprite;

	private float elaspedTime = 0;
	private float nextTime = 0;
	public Economy economy;

	public WarpGate warpGatePrefab;
	private WarpGate warpGate;
	private List<GameObject> createdObjects = new List<GameObject>();

	public ContourRenderer contourRenderer;
	public Shadows shadowsRenderer;

	public ShipSpawner tradeShipSpawner;
	public ShipSpawner colonyShipSpawner;
	public ShipSpawner collectableSpawner;  //will spawn any object which derives from body
	//public Collectable[] collectables;

	//public float popularity = 1f;
	public CollectablesController collectablesController;

	public GUIController guiController;

	public void Visit (Level visitable){
	
	}

	public void Visit (BaseConfiguration visitable){
		
	}

	public void Visit (PlanetConfiguration visitable){

		Planet planet = Instantiate (planetPrefab);
		planet.position = new Vector2 (visitable.Position.X, visitable.Position.Y);
		planet.mass = visitable.Mass;

		if (visitable.Type == PlanetType.Red) {
			planet.SetSprite(redPlanetSprite);
		} else if (visitable.Type == PlanetType.Blue) {
			planet.SetSprite(bluePlanetSprite);
		}else if (visitable.Type == PlanetType.GasGiant) {
			planet.SetSprite(gasPlanetSprite);
		}

		planet.soi = visitable.SOI;
		planet.canMove = false;

		AddResource (planet.gameObject, Cargo.Food, FOOD_BASE_PRICE, FOOD_MAX_PRICE,100, 100,1f);
		AddResource (planet.gameObject, Cargo.People, 10, 10,0, 100,1f); //price is meaningless

		planet.BuildResourceCharts ();
		
		solarSystem.AddBody (planet);
		
		createdObjects.Add (planet.gameObject);
	
	}

	public void Visit (WormHoleConfiguration visitable){
		CreateWarpGate (Cargo.Food, new Vector3(visitable.Position.X,visitable.Position.Y,visitable.Position.Z));
	}

	public void Visit (SunConfiguration visitable){

		Planet sun = Instantiate (planetPrefab);
		sun.SetSprite (sunSprite);
		sun.position = new Vector2 (visitable.Position.X, visitable.Position.Y);
		sun.mass = 1e+25f;
		sun.canMove = false;
		sun.IsLightSource (true);
		sun.imageScale = 1f;
		solarSystem.AddBody (sun);

	}

	public void Reset(){
		guiController.Reset ();
		solarSystem.Clear ();
		foreach (GameObject g in createdObjects) {
			Destroy(g);
		}
		elaspedTime = 0f;
		nextTime = 0f;
		traderShipPool.Clear ();
		colonyShipPool.Clear ();
		ships.Clear ();
		economy.Reset ();
		contourRenderer.Reset ();
		collectablesController.Reset ();
		popularityController.Reset ();
		BuildLevel ();

	}
	public int GetNumberOfShips(){
		return ships.Count;
	}

	Resource AddResource(GameObject p, Cargo type, float baseCost, float maxPrice, int current, int max, float timeToConsumeOneUnit) {
	

		Resource r = p.AddComponent<Resource> ();
		r.max = max;
		r.current = current;
		r.resourceType = type;
		r.basePrice = baseCost;
		r.maxPrice = maxPrice;
		r.timeToConsumeOneUnit = 10000000f; //default value (assumes nobody on planet)
		r.ResourceDepleted += HandleResourceDepleted;
		r.ResourceLevelChanged += HandleResourceLevelChanged;
		return r;
	}




	//when a ship enters a warp gate we add the cargo to this.
	//We then randomly pop off this queue when we spawn 
	List<ShipState> shipStateQueue = new List<ShipState>();

	public struct ShipState
	{
		public ShipState(Cargo type, int volume) {
			this.type = type;
			this.volume = volume;
		}

		public Cargo type;
		public int volume;
	}

	private void CreateWarpGate(Cargo type, Vector3 position){
		WarpGate warpGate = (WarpGate)Instantiate (warpGatePrefab,position, Quaternion.identity);
		warpGate.ShipEnteredWarpGate+= HandleShipEnteredWarpGate;
		//warpGate.cargoType = type;

		warpGate.resource = AddResource (warpGate.gameObject, type, FOOD_BASE_PRICE, FOOD_MAX_PRICE,100000, 100000,1f);
		createdObjects.Add (warpGate.gameObject);
		solarSystem.AddBody (warpGate);
		warpGate.mass = 1;
		warpGate.canMove = false;
		warpGate.position = position;

	}

	private float FOOD_BASE_PRICE = 100f;
	private float FOOD_MAX_PRICE = 500f;

	public void BuildLevel() {
		//position 3 planets ramdomly

		collectablesController.Build ();

		//CreateWarpGate (Cargo.Medical, new Vector3 (4, 3, 0));

		/*
		Planet sun = Instantiate (planetPrefab);
		sun.SetSprite (sunSprite);
		sun.position = new Vector2 (600000f, -100000f);
		sun.mass = 1e+25f;
		sun.canMove = false;
		sun.IsLightSource (true);
		sun.imageScale = 1f;
		solarSystem.AddBody (sun);
		


		//DynamicLight dl = sun.gameObject.AddComponent<DynamicLight> ();
		//dl.lightMaterial = lightMaterial;
		//Gas Giant
		Planet gasGiant = Instantiate (planetPrefab);
		gasGiant.position = new Vector2 (-600000f, -100000f);
		gasGiant.mass = 1e+25f;
		gasGiant.SetSprite(gasPlanetSprite);
		//sun.IsLightSource (false);
		//gasGiant.foodSupplies = 100;
		//gasGiant.maxFoodSupplies = 100;
		//gasGiant.rateOfConsumptionFoodlSupplies = 0.5f;
		gasGiant.soi = 150000;
		gasGiant.canMove = false;
		//gasGiant.ResourceDepleted+= HandleResourceDepleted;

		AddResource (gasGiant.gameObject, Cargo.Food, FOOD_BASE_PRICE, FOOD_MAX_PRICE,100, 100,1f);

		AddResource (gasGiant.gameObject, Cargo.People, 10, 10,0, 100,1f); //price is meaningless


		//AddResource (gasGiant.gameObject, Cargo.Food, 100f, 100, 100,1f);
		gasGiant.BuildResourceCharts ();

		solarSystem.AddBody (gasGiant);

		createdObjects.Add (gasGiant.gameObject);



		
		Planet redPlanet = Instantiate (planetPrefab);
		redPlanet.position = new Vector2 (200000f, 40000);
		redPlanet.mass = 1e+24f;
		redPlanet.SetSprite(redPlanetSprite);
		//redPlanet.foodSupplies = 100;
		//redPlanet.maxFoodSupplies = 100;
		//redPlanet.rateOfConsumptionFoodlSupplies = 0.5f;
		redPlanet.soi = 150000;
		redPlanet.imageScale = 0.75f;
		redPlanet.canMove = false;
		//redPlanet.ResourceDepleted+= HandleResourceDepleted;


		AddResource (redPlanet.gameObject, Cargo.Food, FOOD_BASE_PRICE, FOOD_MAX_PRICE,100, 100,1f);
		AddResource (redPlanet.gameObject, Cargo.People, 10, 10,0, 100,1f); //price is meaningless

		//AddResource (redPlanet.gameObject, Cargo.Food, 100f, 100, 100, 5f);
		//AddResource (redPlanet.gameObject, Cargo.Medical, 100f, 100, 100, 2f);
		redPlanet.BuildResourceCharts ();

		solarSystem.AddBody (redPlanet);

		createdObjects.Add (redPlanet.gameObject);
		
		
		Planet bluePlanet = Instantiate (planetPrefab);
		bluePlanet.position = new Vector2 (700000, 250000);
		bluePlanet.mass = 1e+24f;
		bluePlanet.SetSprite(bluePlanetSprite);
		//bluePlanet.foodSupplies = 100;
		//bluePlanet.maxFoodSupplies = 100;
		//bluePlanet.rateOfConsumptionFoodlSupplies = 0.5f;
		bluePlanet.imageScale = 0.75f;
		bluePlanet.soi = 150000;
		bluePlanet.canMove = false;
		//bluePlanet.ResourceDepleted+= HandleResourceDepleted;

		AddResource (bluePlanet.gameObject, Cargo.Food, FOOD_BASE_PRICE, FOOD_MAX_PRICE,100, 100,1f);
		AddResource (bluePlanet.gameObject, Cargo.People, 10, 10,0, 100,1f); //price is meaningless
		//AddResource (bluePlanet.gameObject, Cargo.Food, 100f, 100, 100,5f);
		bluePlanet.BuildResourceCharts ();


		solarSystem.AddBody (bluePlanet);

		createdObjects.Add (bluePlanet.gameObject);
*/

		colonyShipSpawner.Spawned+= HandleShipSpawned;
		tradeShipSpawner.Spawned+= HandleTradeShipSpawned;

		traderShipPool = PopulatePool<TraderShip> (traderShipPrefab, 1);
		//tradeShipSpawner.shipPool = 

		colonyShipPool = PopulatePool<ColonyShip> (colonyShipPrefab, 10);
		//colonyShipSpawner.shipPool = colonyShipPool;
		//PopulatePool ();
		//PopulateColonyPool ();




		contourRenderer.Build ();

		shadowsRenderer.Build ();

		//create our initial ship
		TraderShip s = traderShipPool[0];
		traderShipPool.RemoveAt (0);
		tradeShipSpawner.Spawn (s);
		s.cargoType = Cargo.Food;
		s.cargo = 100;
		ships.Add (s);
		guiController.Build ();


		audioSource.volume = 1f;
		audioSource.clip = backgroundSound;
		audioSource.Play();

	}

	void HandleTradeShipSpawned (Body ship)
	{

		((TraderShip)ship).cargoType = shipStateQueue [0].type;
		((TraderShip)ship).cargo = shipStateQueue [0].volume;

	}

	void HandleShipSpawned (Body ship)
	{

	}

	// Use this for initialization
	void Start () {

		//BuildLevel ();

		solarSystem.ShipEnteredOrbit+= HandleShipEnteredOrbit;
		colonyShipTimer.TimerEvent += ColonyShipTimerEvent;
		traderShipTimer.TimerEvent += TraderShipTimer;
		popularityController.PopularityChanged += HandlePopularityChanged;
		economy.Profit+= HandleProfit;
	}

	void HandleProfit (float profit)
	{
		if (profit > 0) {
			audioSource.PlayOneShot (profitSound);
		}
	}

	void HandlePopularityChanged (float popularity)
	{
		if (popularity < 0) {
			this.GameOver();
		}
	}

	void TraderShipTimer ()
	{
		if(traderShipPool.Count>0) { // ships in the pool
			TraderShip pooledShip = traderShipPool[0];
			traderShipPool.RemoveAt(0);
			pooledShip.fuel = 1f;
			tradeShipSpawner.Spawn(pooledShip);
		}

	}

	void ColonyShipTimerEvent ()
	{
	
		if(colonyShipPool.Count>0) { // ships in the pool
			ColonyShip pooledShip = colonyShipPool[0];
			pooledShip.fuel = 1f;
			colonyShipPool.RemoveAt(0);
			colonyShipSpawner.Spawn(pooledShip);
		}

	}

	void HandleShipEnteredOrbit (Body s, Body p)
	{
		if (p is Planet && s is ColonyShip) {
			int pop = ((ColonyShip)s).population;
			((Planet)p).AddPopulation(pop);

			//Remove ship from solar system and 
			solarSystem.RemoveBody (s);
			s.gameObject.transform.position = new Vector2(100f,100f);
			s.gameObject.SetActive (false);
			colonyShipPool.Add(((ColonyShip)s));

			//play an applause sound??

			Vector3 position = p.gameObject.transform.position;
			Blades b = (Blades)Instantiate (bladePrefab, position, Quaternion.identity);
			b.color = Helpers.GetCargoColor(Cargo.People);
			audioSource.PlayOneShot (profitSound);
		}
	}



	void HandleResourceDepleted (Resource r, Cargo type)
	{
	
	}

	void HandleResourceLevelChanged (Resource r, float percentage, float change)
	{
		if (r.resourceType == Cargo.Food && change > 0) {

			Vector3 position = r.gameObject.transform.position;
			Blades b = (Blades)Instantiate (bladePrefab, position, Quaternion.identity);
			b.color = 	b.color = Helpers.GetCargoColor(Cargo.Food);
		}
		if (r.resourceType == Cargo.People && change<0){
			//somebody has died
			popularityController.IncrementPopularityBy (-0.1f);
		}
		if (r.resourceType == Cargo.People && change<0 ) {
			
			Vector3 position = r.gameObject.transform.position;
			Blades b = (Blades)Instantiate (bladePrefab, position, Quaternion.identity);
			b.color = Color.red;

			audioSource.PlayOneShot(failSound,1f);
		}

		
	}

	void HandleShipEnteredWarpGate (Ship ship, WarpGate warpGate)
	{


		if (ship is TraderShip) {
			TraderShip trader = (TraderShip)ship;
			traderShipPool.Add (trader);
			solarSystem.RemoveBody (trader);
			trader.transform.position = new Vector2(100f,100f);
			trader.gameObject.SetActive (false);

			if (trader.cargoType == warpGate.resource.resourceType) {
				trader.cargo += economy.BuyAtBasePrice (warpGate.resource, trader.maxCargo - trader.cargo);
			} else {
				trader.cargo = economy.BuyAtBasePrice (warpGate.resource, trader.maxCargo);
			}

			shipStateQueue.Add (new ShipState (trader.cargoType, trader.cargo));

			audioSource.PlayOneShot(warpSound,1f);
		}



		//Hidden (ship.gameObject, true);
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



	List<T> PopulatePool<T>(T prefab, int num) where T:Ship {
		List<T> shipPool = new List<T> ();
		for (int i=0; i<num; i++) {
			T newShip = Instantiate (prefab);
			newShip.position = new Vector2 (10000000f,1000000f);
			//newShip.cargoType = Cargo.Food;
			//newShip.cargo = 100;
			newShip.fuel = 1f;
			shipPool.Add(newShip);
			newShip.gameObject.SetActive(false);
			newShip.ShipCollided+= HandleShipCollided;
			newShip.HullFailure+= HandleHullFailure;

			createdObjects.Add (newShip.gameObject);

			ShipIndicator shipIndicator = Instantiate(shipIndicatorPrefab);
			shipIndicator.ship = newShip;
			createdObjects.Add (shipIndicator.gameObject);
			shipIndicators.Add(shipIndicator);

			//newShip.gameObject.SetActive(false);
		}
		return shipPool;
	}

	void HandleHullFailure (Ship ship)
	{
		DestroyShip (ship);
		CheckShipStatus ();
	}

	private void DestroyShip(Ship ship) {

		ShipIndicator toDestroy = null;
		foreach(ShipIndicator s in shipIndicators){
			if(s.ship==ship){
				toDestroy = s;
				break;
			}	
		}

		if (toDestroy != null) {
			Destroy(toDestroy.gameObject);
		}

		ships.Remove (ship);
		solarSystem.RemoveBody (ship);
	
		//should come from a pool
		Explosion e = (Explosion)Instantiate (explosionPrefab);
		e.transform.position = ship.transform.position;
		e.position = ship.position;
		e.velocity = ship.velocity;
		e.mass = ship.mass;
		e.acceleration = ship.acceleration;
		e.canMove = true;
		e.canAlign = true;
		solarSystem.AddBody (e);
		e.AlignToVector (ship.velocity);
		Destroy (ship.gameObject);


	
	}

	void HandleShipCollided (Ship ship, GameObject other)
	{
	//	return;
		DestroyShip (ship);

		//audioSource.Stop();
		//audioSource.volume = 1f;
		//audioSource.clip = explosionSound;
		//audioSource.PlayOneShot(explosionSound);


		Ship otherShip = other.GetComponent<Ship> ();

		if (otherShip != null) {
			DestroyShip (otherShip);
		} else {
			Destroy (other.gameObject);
		}

		if (other.GetComponent<ColonyShip> () || ship.gameObject.GetComponent<ColonyShip>()) {
			popularityController.IncrementPopularityBy(-0.1f);
		}

		ShipCollided ();

		CheckShipStatus ();
		

	}

	
	public delegate void GameOverHandler();
	public event GameOverHandler GameOver;

	public delegate void ShipCollidedHandler();
	public event ShipCollidedHandler ShipCollided;

	public delegate void PopularityChangedHandler(float popularity);
	public event PopularityChangedHandler PopularityChanged;
	


	public Vector2 CalculateScreenSizeInWorldCoords ()  {
		var cam = Camera.main;
		Vector3 p1 = cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane));  
		Vector3 p2 = cam.ViewportToWorldPoint(new Vector3(1,0,cam.nearClipPlane));
		Vector3 p3 = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane));
		
		float width = (p2 - p1).magnitude;
		float height = (p3 - p2).magnitude;

		Debug.Log (width + " " + height);
		
		Vector2 dimensions = new Vector2(width,height);
		
		return dimensions;
	}



	private void CheckShipStatus() {
		if (ships.Count == 0) {
			GameOver ();
		}
	}

	// Update is called once per frame
	void Update () {



	}
}
