using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour, IGameController, IWinCondition, IStartStop {

	public GuesturesHandler gestureHandler;

	public CameraFit cameraFitter;
	public event WinConditionHandler Win;
	public RepeatSpriteBoundary gridController;
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
//	private IList<ColonyShip> colonyShipPool = new List<ColonyShip> ();
	public Material lightMaterial;
	public const float SCALE = 1000000;
	public BodyController cometController;
	public BodyController asteriodController;
	public ShipController colonyShipController;
	public ShipController cargoShipController;
	public ShipController turretController;
	public StarsController starController;
	public BackgroundController backgroundController;

	public BulletController bulletController;
	private IList<ShipIndicator> shipIndicators = new List<ShipIndicator> ();

	private List<IStartStop> stoppables = new List<IStartStop>();
	
	public int maxNumShips = 1;

	public TraderShip traderShipPrefab;

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

	public ShipSpawner tradeShipSpawner;
	
	//public CollectablesController collectablesController;


	public GUIController guiController;


	public PlayerDataController playerDataController;

	public List<IWinCondition> winConditions = new List<IWinCondition>();
	public List<IFailCondition> failConditions = new List<IFailCondition>();

	public delegate void GameOverHandler(string message);
	public event GameOverHandler GameOver;

	public void OnGameOver (string message) {

		if (GameOver != null) {
			GameOver(message);
		}

		foreach (IStartStop s in stoppables) {
			s.StopPlay();
		}
	}

	
	public delegate void ShipCollidedHandler();
	public event ShipCollidedHandler ShipDestroyed;
	
	public delegate void PopularityChangedHandler(float popularity);
	public event PopularityChangedHandler PopularityChanged;

	private Vector2 startPosition = new Vector2();
	public void Visit (Level visitable){
		cameraFitter.UnitsForWidth = visitable.Scale;
		cameraFitter.ComputeResolution ();


		if (visitable.Position != null) {
			startPosition = new Vector2(visitable.Position.X,visitable.Position.Y);
		}


	}

	public void Visit (SpawnConfiguration visitable){

		float worldScreenHeight = (float)(Camera.main.orthographicSize * 2.0);
		float worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width);
		
		float margin = 5f;
		//mutate the config
		switch(visitable.Position.from){
			case From.Top:
				
				visitable.Position.X = visitable.Position.X /GameController.SCALE;
				visitable.Position.Y =  (worldScreenHeight/2f) + margin;
				break;
				
			case From.Bottom:
				visitable.Position.X = visitable.Position.X /GameController.SCALE;
				visitable.Position.Y =  -(worldScreenHeight/2f) - margin;
				break;
				
			case From.Left:
				visitable.Position.Y = visitable.Position.Y /GameController.SCALE;
				visitable.Position.X =  -(worldScreenWidth/2f) - margin;
				break;
				
			case From.Right:
				visitable.Position.Y = visitable.Position.Y /GameController.SCALE;
				visitable.Position.X =  (worldScreenWidth/2f) + margin;
				break;
			default:
				visitable.Position.Y = visitable.Position.Y /GameController.SCALE;
				visitable.Position.X = visitable.Position.X /GameController.SCALE;
			break;
				
		}


		if (visitable.SpawnType == SpawnType.ColonyShip) {
			((ConfigurableSpawnRequester)colonyShipController.spawnRequester).Add (visitable);
		} else if (visitable.SpawnType == SpawnType.Comet) {
			((ConfigurableSpawnRequester)cometController.spawnRequester).Add (visitable);
		} else if (visitable.SpawnType == SpawnType.CargoShip) {
			((ConfigurableSpawnRequester)cargoShipController.spawnRequester).Add (visitable);
		} else if (visitable.SpawnType == SpawnType.Turret) {
			((ConfigurableSpawnRequester)turretController.spawnRequester).Add (visitable);
		} else if (visitable.SpawnType == SpawnType.Asteriod) {
			((ConfigurableSpawnRequester)asteriodController.spawnRequester).Add (visitable);
		}
		
	}

	public void Visit (BaseConfiguration visitable){
		
	}

	public void Visit (ConstellationLineConfiguration visitable){
		
	}

	public void Visit (LevelMapItemConfiguration visitable){
		
	}

	public void Visit (PlanetResourceConfiguration visitable){
		AddResource (planetUnderConstruction.gameObject, visitable.ResourceType, FOOD_BASE_PRICE, FOOD_MAX_PRICE,visitable.Current, visitable.Max,1f);
	}

	private Planet planetUnderConstruction;
	public void Visit (PlanetConfiguration visitable){

		Planet planet = Instantiate (planetPrefab);
		planetUnderConstruction = planet;
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
		sun.mass = 6e+27f;
		sun.canMove = false;
		sun.SetIsLightSource (true);
		sun.imageScale = 1f;
		solarSystem.AddBody (sun);
		createdObjects.Add (sun.gameObject);

	}

	public void Reset(){

		cometController.BodyBuilt -= HandleCometBodyBuilt;
		colonyShipController.BodyBuilt -= HandleColonyShipBodyBuilt;
		cargoShipController.BodyBuilt -= HandleColonyShipBodyBuilt;
		turretController.BodyBuilt -= HandleColonyShipBodyBuilt;
		asteriodController.BodyBuilt -= HandleCometBodyBuilt;

		gestureHandler.SelectionChanged -= HandleSelectionChanged;
		starController.Reset ();
		solarSystem.Reset ();
		stoppables.Clear ();
		winConditions.Clear ();
		failConditions.Clear ();
		guiController.Reset ();
		guiController.gameObject.SetActive (false);
		solarSystem.Clear ();

		elaspedTime = 0f;
		nextTime = 0f;
		traderShipPool.Clear ();
		//colonyShipPool.Clear ();
		ships.Clear ();
		economy.Reset ();
		contourRenderer.Reset ();
		for (int i=0; i<collectables.Length; i++) {
			collectables[i].Reset();
		}
		//collectablesController.Reset ();
		popularityController.Reset ();
		foreach (GameObject g in createdObjects) {
			Destroy(g);
		}
		createdObjects.Clear();

		bulletController.Reset ();
		cometController.Reset ();
		asteriodController.Reset ();
		colonyShipController.Reset ();
		cargoShipController.Reset ();
		turretController.Reset ();
		gridController.Reset ();
		//BuildLevel ();

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

		stoppables.Add (r);

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
		warpGate.BodyEnteredWarpGate+= HandleBodyEnteredWarpGate;
		//warpGate.cargoType = type;

		warpGate.resource = AddResource (warpGate.gameObject, type, FOOD_BASE_PRICE, FOOD_MAX_PRICE,100000, 100000,1f);
		createdObjects.Add (warpGate.gameObject);
		solarSystem.AddBody (warpGate);
		warpGate.mass = 1;
		warpGate.canMove = false;
		warpGate.position = position;
		stoppables.Add (warpGate);

	}

	private float FOOD_BASE_PRICE = 100f;
	private float FOOD_MAX_PRICE = 500f;


	public void Build(Ready ready) {

		gestureHandler.SelectionChanged+= HandleSelectionChanged;

		int count=0;
		Ready Done = delegate() {
			count++;
			if(count==15 + collectables.Length){
				//start sound and show gui
				guiController.gameObject.SetActive (true);
				ready();
			}
		};

	

		stoppables.Add (solarSystem);
		stoppables.Add (asteriodController);
		stoppables.Add(cometController);
		stoppables.Add (colonyShipController);
		stoppables.Add (cargoShipController);
		stoppables.Add (turretController);
		stoppables.Add (bulletController);
		stoppables.Add (starController);

		Vector2 topRight = new Vector2 (1, 1);
		Vector2 edgeVector = Camera.main.ViewportToWorldPoint (topRight);
		Vector2 worldBounds = new Vector2 (edgeVector.x * GameController.SCALE, edgeVector.y * GameController.SCALE);
		
		solarSystem.SetWorldBounds (worldBounds);

		//position 3 planets ramdomly
		//collectablesController.Build (Done);

		for (int i=0; i<collectables.Length; i++) {
			stoppables.Add(collectables[i]);
			collectables[i].Build(Done);
		}

		tradeShipSpawner.Spawned+= HandleTradeShipSpawned;

		traderShipPool = PopulatePool<TraderShip> (traderShipPrefab, 1);

		//colonyShipPool = PopulatePool<ColonyShip> (colonyShipPrefab, 10);

		contourRenderer.Build (Done);

		//Trader ship shouldnt be spawned
		TraderShip s = traderShipPool[0];
		traderShipPool.RemoveAt (0);
		Vector2 position = new Vector2 ();
		Vector2 velocity = new Vector2 ();
		//tradeShipSpawner.Spawn (ref position,ref velocity);
		s.position = startPosition;
		s.lastPosition = s.position - (velocity * Time.fixedDeltaTime);
		s.gameObject.transform.position = startPosition/GameController.SCALE;
		solarSystem.AddBody(s);
		s.gameObject.SetActive(true);

		s.cargoType = Cargo.Food;
		s.cargo = 100;
		ships.Add (s);

		playersActiveShip = s;


		guiController.Build (Done);

		gridController.Build (Done);


		//lets add the win conditions. These may in the future be configured
		PlanetsPopulatedWinCondition winCondition = new PlanetsPopulatedWinCondition ();
		winCondition.solarSystem = solarSystem;
		winCondition.economy = economy;
		winCondition.Build (Done);
		winCondition.Win += HandleWin;
		winConditions.Add (winCondition);

		PlanetDeadFailCondition planetDeadFailCondition = new PlanetDeadFailCondition ();
		planetDeadFailCondition.solarSystem = solarSystem;
		planetDeadFailCondition.Build (Done);
		planetDeadFailCondition.Fail += HandleFail;
		failConditions.Add (planetDeadFailCondition);

		AllColonyShipsAccountedForFailCondition allColonyShipsAccountedForFailCondition = new AllColonyShipsAccountedForFailCondition ();
		allColonyShipsAccountedForFailCondition.solarSystem = solarSystem;
		allColonyShipsAccountedForFailCondition.gameController = this;
		allColonyShipsAccountedForFailCondition.colonyShipController = colonyShipController;
		allColonyShipsAccountedForFailCondition.Build (Done);
		allColonyShipsAccountedForFailCondition.Fail += HandleFail;
		failConditions.Add (allColonyShipsAccountedForFailCondition);



		AllShipsDestroyedFailCondition allShipsFailCondition = new AllShipsDestroyedFailCondition ();
		allShipsFailCondition.gameController = this;
		allShipsFailCondition.Build (Done);
		allShipsFailCondition.Fail += HandleFail;
		failConditions.Add (allShipsFailCondition);


		audioSource.volume = 1f;
		audioSource.clip = backgroundSound;
		audioSource.Play();

	

		foreach (Body b in solarSystem.bodies) {
			if(b is Planet){
				((Planet)b).BuildResourceCharts ();
			}
		}

		//colonyShipTimer.Play ();
		traderShipTimer.Play ();

		bulletController.Build (Done);
		//bulletController.BodyBuilt += HandleBulletBodyBuilt;

		asteriodController.BodyBuilt += HandleCometBodyBuilt;
		asteriodController.Build (Done);

		cometController.BodyBuilt+= HandleCometBodyBuilt;
		cometController.Build (Done);

		backgroundController.Build (Done);
		starController.Build (Done);

		colonyShipController.BodyBuilt+= HandleColonyShipBodyBuilt;
		colonyShipController.Build (Done);

		cargoShipController.BodyBuilt += HandleColonyShipBodyBuilt;
		cargoShipController.Build (Done);

		turretController.BodyBuilt += HandleColonyShipBodyBuilt;
		turretController.Build (Done);

	}

	void HandleColonyShipBodyBuilt(Body b){
		((Ship)b).ShipCollided+= HandleShipCollided;
		((Ship)b).HullFailure+= HandleHullFailure;

		ShipIndicator shipIndicator = Instantiate(shipIndicatorPrefab);
		shipIndicator.ship = (Ship)b;
		createdObjects.Add (shipIndicator.gameObject);
		shipIndicators.Add(shipIndicator);
	}

	void HandleBulletBodyBuilt(Body body){
	//((Ship)b).ShipCollided+= HandleShipCollided;
	}

	void HandleCometBodyBuilt (Body body)
	{
	}

	void HandleSelectionChanged (GameObject g)
	{
		if (playersActiveShip != null && g!=playersActiveShip.gameObject) {
			playersActiveShip.GetComponent<TractorBeam>().TryTractor(g);
			
		}
	}

	public TraderShip playersActiveShip;

	private bool stop = false;

	public void StopPlay(){
		stop = true;
		foreach(IStartStop s in stoppables)
		{
			s.StopPlay();
		}
	}

	
	public void StartPlay(){
		stop = false;
		foreach(IStartStop s in stoppables)
		{
			s.StartPlay();
		}
	}


	void HandleWin (WinData winData)
	{
		Win (winData);
		StopPlay ();

	}

	void HandleFail(string message){
		this.OnGameOver(message);
	}

	void HandleTradeShipSpawned (Body ship)
	{

		((TraderShip)ship).cargoType = shipStateQueue [0].type;
		((TraderShip)ship).cargo = shipStateQueue [0].volume;

	}

	private CollectablesController[] collectables;

	// Use this for initialization
	void Start () {
		//BuildLevel ();


		collectables = FindObjectsOfType<CollectablesController> ();

		solarSystem.ShipEnteredOrbit+= HandleShipEnteredOrbit;
		//colonyShipTimer.TimerEvent += ColonyShipTimerEvent;
		traderShipTimer.TimerEvent += TraderShipTimer;
		popularityController.PopularityChanged += HandlePopularityChanged;
		economy.Profit+= HandleProfit;

	//	Vector2 topRight = new Vector2 (1, 1);
	//	Vector2 edgeVector = Camera.main.ViewportToWorldPoint (topRight);
	//	Vector2 worldBounds = new Vector2 (edgeVector.x * GameController.SCALE, edgeVector.y * GameController.SCALE);
		
	//	solarSystem.SetWorldBounds (worldBounds);

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
			this.OnGameOver("popularity is zero" );
		}
	}


	void TraderShipTimer ()
	{
		if (stop)
			return;

		if(traderShipPool.Count>0) { // ships in the pool
			TraderShip pooledShip = traderShipPool[0];
			traderShipPool.RemoveAt(0);
			pooledShip.fuel = 1f;
			//tradeShipSpawner.Spawn(pooledShip);
			Vector2 position = new Vector2 ();
			Vector2 velocity = new Vector2 ();
			tradeShipSpawner.Spawn (ref position,ref velocity);
			pooledShip.position = startPosition;
			pooledShip.lastPosition = pooledShip.position - (velocity * Time.fixedDeltaTime);
			solarSystem.AddBody(pooledShip);
			pooledShip.gameObject.SetActive (true);

		}

	}
	

	void HandleShipEnteredOrbit (Body s, Body p)
	{

		if (s is Comet || s is Asteriod) {
		
			DestroyBody(s);

			((Planet)p).ClearResources();

		}

		if (s is Ship && s.velocity.magnitude > SolarSystem.MAX_RENTRY_SPEED) {
			//travelling too fast
			((Ship)s).hull=0;
			return;
		}

		economy.HandleShipEnteredOrbit (s, p);

		if (p is Planet && s is ColonyShip) {
	
			//Remove ship from solar system and 
			//solarSystem.RemoveBody (s);
			//s.gameObject.transform.position = new Vector2(100f,100f);
			//s.gameObject.SetActive (false);
			colonyShipController.ReturnToPool(s);
	
			//play an applause sound??

			Vector3 position = p.gameObject.transform.position;
			Blades b = (Blades)Instantiate (bladePrefab, position, Quaternion.identity);
			b.color = Helpers.GetCargoColor(Cargo.People);
			createdObjects.Add(b.gameObject);
			audioSource.PlayOneShot (profitSound);

			//immediately spawn another colony ship

			colonyShipController.SpawnNow();

		}

		if (p is Planet && s is CargoShip) {
			
			//Remove ship from solar system and 
			//solarSystem.RemoveBody (s);
			//s.gameObject.transform.position = new Vector2(100f,100f);
			//s.gameObject.SetActive (false);
			cargoShipController.ReturnToPool(s);
			
			//play an applause sound??
			
			Vector3 position = p.gameObject.transform.position;
			Blades b = (Blades)Instantiate (bladePrefab, position, Quaternion.identity);
			b.color = Helpers.GetCargoColor(Cargo.Food);
			createdObjects.Add(b.gameObject);
			audioSource.PlayOneShot (profitSound);
			
			//immediately spawn another colony ship
			
			cargoShipController.SpawnNow();
			
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

	void HandleBodyEnteredWarpGate (Body body, WarpGate warpGate)
	{


		if (body is TraderShip) {

			//warp gate is no longer source of food
			ships.Remove ((Ship)body);
			audioSource.PlayOneShot (warpSound, 1f);


			//DestroyBody(body);

			/*
			TraderShip trader = (TraderShip)body;
			traderShipPool.Add (trader);
			solarSystem.RemoveBody (trader);
			trader.transform.position = new Vector2 (100f, 100f);
			trader.gameObject.SetActive (false);

			if (trader.cargoType == warpGate.resource.resourceType) {
				trader.cargo += economy.BuyAtBasePrice (warpGate.resource, trader.maxCargo - trader.cargo);
			} else {
				trader.cargo = economy.BuyAtBasePrice (warpGate.resource, trader.maxCargo);
			}

			shipStateQueue.Add (new ShipState (trader.cargoType, trader.cargo));

			audioSource.PlayOneShot (warpSound, 1f);
			*/
		} else  {
			solarSystem.RemoveBody (body);
			body.gameObject.SetActive(false);
			audioSource.PlayOneShot (warpSound, 1f);
		}

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
			createdObjects.Add(shipIndicator.gameObject);

		}
		return shipPool;
	}

	void HandleHullFailure (Ship ship)
	{
		DestroyBody (ship);
	}

	private BodyController GetController(Body body){
		if (body is Comet) {
			return cometController;
		} if (body is CargoShip) {
			return cargoShipController;
		}if (body is Bullet) {
			return bulletController;
		} else if (body is Asteriod) {
			return asteriodController;
		}
		else if (body is ColonyShip) {
			return colonyShipController;
		} else if (body is Turret) {
			return turretController;
		}else {
			return null;
		}
	}

	private void DestroyBody(Body body) {


		if (body is Ship) {
			ShipIndicator toDestroy = null;
			foreach (ShipIndicator s in shipIndicators) {
				if (s.ship == (Ship)body) {
					toDestroy = s;
					break;
				}	
			}
			
			if (toDestroy != null) {
				Destroy(toDestroy.gameObject);
			}


		}

		BodyController bc = GetController (body);
		if(bc)
			bc.ReturnToPool (body);

		//should come from a pool
		Debug.Log ("created explosion");
		Explosion e = (Explosion)Instantiate (explosionPrefab);
		e.transform.position = body.transform.position;
		e.position = body.position;
		e.lastPosition = body.lastPosition;
		e.mass = body.mass;
		e.acceleration = body.acceleration;
		e.canMove = true;
		e.canAlign = true;
		solarSystem.AddBody (e);
		e.AlignToVector (body.velocity);

		createdObjects.Add (e.gameObject);

		StartCoroutine(DestroyExplosion (e));

		if(body is TraderShip) {
			solarSystem.RemoveBody(body);
			ships.Remove ((Ship)body);
			ShipDestroyed ();
		}
	
	}

	IEnumerator DestroyExplosion(Explosion e) {
		yield return new WaitForSeconds(3);
		if (solarSystem.ContainsBody(e)) {
			Debug.Log("destroyed explosion");
			solarSystem.RemoveBody (e);
			createdObjects.Remove (e.gameObject);
			Destroy (e.gameObject);
		}
	}
	
	
	void HandleShipCollided (Ship ship, Body other)
	{

		if (ship is Bullet || other is Bullet) {
			
			Debug.Log("bullet in collision");
		}

		if (ship is Turret && other is Bullet) {
			return;
		}

		if (other is Planet || other is Collectable || other is WarpGate || other is Explosion ) {
			return;
		}



		DestroyBody (ship);

		if (other != null) {
			DestroyBody (other);
		} else {
			Destroy (other.gameObject);
		}

		if (other is ColonyShip || ship is ColonyShip) {
			popularityController.IncrementPopularityBy(-0.1f);
		}
	
	}

	/*
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

*/
}
