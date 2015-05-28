using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipSpawner : MonoBehaviour {

	private IList<Ship> shipPool = new List<Ship> ();
	//private int shipPoolSize = 5;
	public int maxNumShips = 1;
	public Ship shipPrefab;
	public SolarSystem solarSystem;
	public Planet planetPrefab;
	public Sprite gasPlanetSprite;
	public Sprite bluePlanetSprite;
	public Sprite redPlanetSprite;

	private float elaspedTime = 0;
	private float nextTime = 0;
	public Economy economy;

	public WarpGate warpGatePrefab;
	private WarpGate warpGate;
	private List<GameObject> createdObjects = new List<GameObject>();


	public void Reset(){
		solarSystem.Clear ();
		foreach (GameObject g in createdObjects) {
			Destroy(g);
		}
		elaspedTime = 0f;
		nextTime = 0f;
		shipPool.Clear ();
		BuildLevel ();
		economy.Reset ();
	}

	void AddResource(Planet p, Cargo type, float cost, int current, int max, float timeToConsumeOneUnit) {
	
		Resource r = p.gameObject.AddComponent<Resource> ();
		r.max = max;
		r.current = current;
		r.resourceType = type;
		r.price = cost;
		r.timeToConsumeOneUnit = timeToConsumeOneUnit;
	}

	void BuildLevel() {
		//position 3 planets ramdomly
		
		warpGate = (WarpGate)Instantiate (warpGatePrefab, new Vector3(-2,1,0), Quaternion.identity);
		warpGate.ShipEnteredWarpGate+= HandleShipEnteredWarpGate;

		createdObjects.Add (warpGate.gameObject);



		//Gas Giant
		Planet gasGiant = Instantiate (planetPrefab);
		gasGiant.position = new Vector2 (-600000f, -100000f);
		gasGiant.mass = 1e+25f;
		gasGiant.GetComponent<SpriteRenderer> ().sprite = gasPlanetSprite;
		//gasGiant.foodSupplies = 100;
		//gasGiant.maxFoodSupplies = 100;
		//gasGiant.rateOfConsumptionFoodlSupplies = 0.5f;
		gasGiant.soi = 200000;
		gasGiant.canMove = false;
		gasGiant.ResourceDepleted+= HandleResourceDepleted;

		AddResource (gasGiant, Cargo.Food, 100f, 100, 100,1f);
		gasGiant.BuildResourceCharts ();

		solarSystem.AddBody (gasGiant);

		createdObjects.Add (gasGiant.gameObject);
		
		
		Planet redPlanet = Instantiate (planetPrefab);
		redPlanet.position = new Vector2 (200000f, 40000);
		redPlanet.mass = 1e+20f;
		redPlanet.GetComponent<SpriteRenderer> ().sprite = redPlanetSprite;
		//redPlanet.foodSupplies = 100;
		//redPlanet.maxFoodSupplies = 100;
		//redPlanet.rateOfConsumptionFoodlSupplies = 0.5f;
		redPlanet.soi = 100000;
		redPlanet.canMove = false;
		redPlanet.ResourceDepleted+= HandleResourceDepleted;

		AddResource (redPlanet, Cargo.Food, 100f, 100, 100, 5f);
		redPlanet.BuildResourceCharts ();

		solarSystem.AddBody (redPlanet);

		createdObjects.Add (redPlanet.gameObject);
		
		
		Planet bluePlanet = Instantiate (planetPrefab);
		bluePlanet.position = new Vector2 (700000, 400000);
		bluePlanet.mass = 1e+24f;
		bluePlanet.GetComponent<SpriteRenderer> ().sprite = bluePlanetSprite;
		//bluePlanet.foodSupplies = 100;
		//bluePlanet.maxFoodSupplies = 100;
		//bluePlanet.rateOfConsumptionFoodlSupplies = 0.5f;
		
		bluePlanet.soi = 150000;
		bluePlanet.canMove = false;
		bluePlanet.ResourceDepleted+= HandleResourceDepleted;

		AddResource (bluePlanet, Cargo.Food, 100f, 100, 100,5f);
		bluePlanet.BuildResourceCharts ();


		solarSystem.AddBody (bluePlanet);

		createdObjects.Add (bluePlanet.gameObject);

		PopulatePool ();
		

	}

	// Use this for initialization
	void Start () {

		BuildLevel ();

	}

	void HandleResourceDepleted (Cargo type)
	{
		this.GameOver();
	}

	void HandleShipEnteredWarpGate (Ship ship)
	{
		shipPool.Add (ship);
		solarSystem.RemoveBody (ship);
		ship.transform.position = new Vector2(100f,100f);
		ship.gameObject.SetActive (false);
		//Hidden (ship.gameObject, true);
	}

	public void Hidden(GameObject gameObject, bool isHidden)
	{
		Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
		foreach (Renderer r in renderers)
		{
			r.enabled = !isHidden;
		}
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

	void PopulatePool() {
		for (int i=0; i<maxNumShips; i++) {
			Ship newShip = Instantiate (shipPrefab);
			newShip.position = new Vector2 (10000000f,1000000f);
			newShip.cargoType = Cargo.Food;
			newShip.cargo = 100;
			newShip.fuel = 1f;
			shipPool.Add(newShip);
			newShip.gameObject.SetActive(false);
			newShip.ShipCollided+= HandleShipCollided;

			
			createdObjects.Add (newShip.gameObject);
		}
	}

	void HandleShipCollided (Ship ship, GameObject other)
	{
		GameOver ();
	}

	
	public delegate void GameOverHandler();
	public event GameOverHandler GameOver;
	

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

	// Update is called once per frame
	void Update () {

		if (elaspedTime > nextTime) {

			nextTime = Random.Range(5,10);
			elaspedTime = 0f;

			Vector2 dimensions = CalculateScreenSizeInWorldCoords();

			if(shipPool.Count>0) { // ships in the pool
				Ship pooledShip = shipPool[0];
				shipPool.RemoveAt(0);

				int side = Random.Range(0,3);
				float x = 0;
				float y = 0;

				Vector2 accn = new Vector2();
				float velocity = 100000f;
				if(side==0) {
					//going from top

					x =  Random.Range(-dimensions.x/2f,dimensions.x/2f);
					y = dimensions.y/2f;
					accn = new Vector2(0,-velocity);


				} else if(side==1){
					//going from right
					x = dimensions.x/2f;
					y = Random.Range(-dimensions.y/2f,dimensions.y/2f);
					accn = new Vector2(-velocity,0f);

				} else if(side==2){
					//going from bottom
					x = Random.Range(-dimensions.x/2f,dimensions.x/2f);
					y = -dimensions.y/2f;
					accn = new Vector2(0f,-velocity);
				} else if(side==3){
					//going from left
					x = -dimensions.x/2f;
					y = Random.Range(-dimensions.y/2f,dimensions.y/2f);
					accn = new Vector2(velocity,0f);
				}

				float scale = 100000f;
				pooledShip.velocity = accn;
				pooledShip.AlignToVector(accn);
				pooledShip.position = new Vector2(x * scale,y * scale);
				solarSystem.AddBody(pooledShip);
				pooledShip.gameObject.SetActive(true);

			}

			//warped out ships are added back to the pool
		}
		elaspedTime += Time.deltaTime;
	}
}
