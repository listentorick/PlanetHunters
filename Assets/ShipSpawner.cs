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


	//when a ship enters a warp gate we add the cargo to this.
	//We then randomly pop off this queue when we spawn 
	List<Cargo> cargoQueue = new List<Cargo>();


	private void CreateWarpGate(Cargo type, Vector3 position){
		WarpGate warpGate = (WarpGate)Instantiate (warpGatePrefab,position, Quaternion.identity);
		warpGate.ShipEnteredWarpGate+= HandleShipEnteredWarpGate;
		warpGate.cargoType = type;
		
		createdObjects.Add (warpGate.gameObject);

	}

	void BuildLevel() {
		//position 3 planets ramdomly
		

		CreateWarpGate (Cargo.Food, new Vector3 (-2, 1, 0));
		CreateWarpGate (Cargo.Medical, new Vector3 (4, 3, 0));

		//Gas Giant
		Planet gasGiant = Instantiate (planetPrefab);
		gasGiant.position = new Vector2 (-600000f, -100000f);
		gasGiant.mass = 2e+25f;
		gasGiant.GetComponent<SpriteRenderer> ().sprite = gasPlanetSprite;
		//gasGiant.foodSupplies = 100;
		//gasGiant.maxFoodSupplies = 100;
		//gasGiant.rateOfConsumptionFoodlSupplies = 0.5f;
		gasGiant.soi = 150000;
		gasGiant.canMove = false;
		gasGiant.ResourceDepleted+= HandleResourceDepleted;

		AddResource (gasGiant, Cargo.Food, 100f, 100, 100,1f);
		gasGiant.BuildResourceCharts ();

		solarSystem.AddBody (gasGiant);

		createdObjects.Add (gasGiant.gameObject);
		
		
		Planet redPlanet = Instantiate (planetPrefab);
		redPlanet.position = new Vector2 (200000f, 40000);
		redPlanet.mass = 1e+24f;
		redPlanet.GetComponent<SpriteRenderer> ().sprite = redPlanetSprite;
		//redPlanet.foodSupplies = 100;
		//redPlanet.maxFoodSupplies = 100;
		//redPlanet.rateOfConsumptionFoodlSupplies = 0.5f;
		redPlanet.soi = 150000;
		redPlanet.canMove = false;
		redPlanet.ResourceDepleted+= HandleResourceDepleted;

		AddResource (redPlanet, Cargo.Food, 100f, 100, 100, 5f);
		AddResource (redPlanet, Cargo.Medical, 100f, 100, 100, 2f);
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

	void HandleShipEnteredWarpGate (Ship ship, WarpGate warpGate)
	{
		shipPool.Add (ship);
		solarSystem.RemoveBody (ship);
		ship.transform.position = new Vector2(100f,100f);
		ship.gameObject.SetActive (false);
		cargoQueue.Add (warpGate.cargoType);
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
		Destroy (ship.gameObject);
		Destroy (other.gameObject);
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


	bool HasClearPath(Vector2 position, Vector2 direction) {
		Debug.Log("testing path " + position.x + " " + position.y + " " +direction.x + " " + direction.y);
		Debug.DrawRay (new Vector3 (position.x, position.y, 0), direction, Color.green,2f);
		bool hasCollision = Physics2D.Raycast (new Vector3 (position.x, position.y), direction.normalized);
		Debug.Log (!hasCollision);
		return !hasCollision;
	}

	void SpawnThisShip(Ship ship) {
		Vector2 position = new Vector2 ();
		Vector2 velocity = new Vector2 ();
		PickPositionAndDirection (ref position, ref velocity);
		for(var i=0; i<10;i++) {
			if(!HasClearPath(position,velocity)) {
				Debug.Log("collision at " + position.x + " " + position.y);
				PickPositionAndDirection (ref position, ref velocity);
			}else {
				Debug.Log("spawn at " + position.x + " " + position.y + " " + velocity.x + " " + velocity.y);
				float scale = 100000f;
				ship.velocity = velocity;
				ship.AlignToVector(velocity);
				ship.position = position * scale;
				ship.gameObject.transform.position = new Vector3(-100,-100,-8); //set start position to ensure z value is correct
				solarSystem.AddBody(ship);
				ship.gameObject.SetActive(true);
				ship.fuel = 1f;
				if(cargoQueue.Count>0) {
					
					ship.cargoType = cargoQueue[0];
					ship.cargo = ship.maxCargo;
					cargoQueue.RemoveAt(0);
				}
				break;
			
			}
		}
	
	}
	//Vector2 P


	void PickPositionAndDirection( ref Vector2 position, ref Vector2 velocity) {
			Vector2 dimensions = CalculateScreenSizeInWorldCoords();
	
			int side = Random.Range(0,4);
			float x = 0;
			float y = 0;
			
			Vector2 accn = new Vector2();
			float velocityMagnitude = 50000f;
			if(side==0) {
				//going from top
				
				x =  Random.Range(-dimensions.x/2f,dimensions.x/2f);
				y = dimensions.y/2f;
				accn = new Vector2(0,-velocityMagnitude);
				
				
			} else if(side==1){
				//going from right
				x = dimensions.x/2f;
				y = Random.Range(-dimensions.y/2f,dimensions.y/2f);
				accn = new Vector2(-velocityMagnitude,0f);
				
			} else if(side==2){
				//going from bottom
				x = Random.Range(-dimensions.x/2f,dimensions.x/2f);
				y = -dimensions.y/2f;
				accn = new Vector2(0f,velocityMagnitude);
			} else if(side==3){
				//going from left
				x = -dimensions.x/2f;
				y = Random.Range(-dimensions.y/2f,dimensions.y/2f);
				accn = new Vector2(velocityMagnitude,0f);
			}
		Debug.Log ("spawn side is " + side);
		position.Set (x, y);
		velocity.Set(accn.x,accn.y);

	}

	// Update is called once per frame
	void Update () {

		if (elaspedTime > nextTime) {

			nextTime = Random.Range(5,10);
			elaspedTime = 0f;

			if(shipPool.Count>0) { // ships in the pool
				Ship pooledShip = shipPool[0];
				shipPool.RemoveAt(0);

				SpawnThisShip(pooledShip);
			}

			//warped out ships are added back to the pool
		}
		elaspedTime += Time.deltaTime;
	}
}
