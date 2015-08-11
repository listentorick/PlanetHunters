using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelMapController : MonoBehaviour, IReset,ILevelConfigurationVisitor, IBuild {

	public SolarSystem solarSystem;
	public const float SCALE = 1000000;
	public Planet planetPrefab;
	public Sprite gasPlanetSprite;
	public Sprite bluePlanetSprite;
	public Sprite redPlanetSprite;
	public Sprite sunSprite;
	public ContourRenderer contourRenderer;
	private List<GameObject> createdObjects = new List<GameObject>();
	public Timer cometTimer;
	public Body cometPrefab;
	public ShipSpawner cometSpawner;
	public Pool cometPool;
	public Line linePrefab;

	void Start() {
		cometTimer.TimerEvent+= HandleTimerEvent;
	}

	void HandleTimerEvent ()
	{
		GameObject g = cometPool.GetPooledObject ();
		if (g!=null) {
			cometSpawner.Spawn (g.GetComponent<Comet> ());
		}
	}

	public void Visit (Level visitable){
		
	}
	
	public void Visit (BaseConfiguration visitable){

	}

	public void Visit (ConstellationLineConfiguration visitable){



		
	//	for (int i = 0; i<solarSystem.bodies.Count; i+=2) {
		Line line = (Line)Instantiate(linePrefab);
		line.target1 = solarSystem.bodies [visitable.index1];
		line.target2 = solarSystem.bodies [visitable.index2];
		createdObjects.Add (line.gameObject);
		//}

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

		//Calculate world bones
	
		
		
		solarSystem.AddBody (planet);
		
		createdObjects.Add (planet.gameObject);
		
	}

	void HandleSelect (GameObject g)
	{
		//g.GetComponent<LevelCon
		LevelSelected ("level1");
	}

	public delegate void LevelSelectedHandler(string levelName);
	public event LevelSelectedHandler LevelSelected;

	
	public void Visit (WormHoleConfiguration visitable){
	}
	
	public void Visit (SunConfiguration visitable){
		
		Planet sun = Instantiate (planetPrefab);
		sun.SetSprite (sunSprite);
		sun.position = new Vector2 (visitable.Position.X, visitable.Position.Y);
		sun.mass = 1e+25f;
		sun.canMove = false;
		//sun.IsLightSource (true);
		sun.imageScale = 1f;
		solarSystem.AddBody (sun);
		createdObjects.Add (sun.gameObject);

		Selectable s = sun.gameObject.AddComponent<Selectable> ();
		s.Select += HandleSelect;
		

		
	}

	public void Reset(){
		solarSystem.Clear ();
		cometPool.Reset ();
		contourRenderer.Reset ();
		foreach (GameObject g in createdObjects) {
			Destroy(g);
		}
	}

	public void Build(){
		Vector2 topRight = new Vector2 (1, 1);
		Vector2 edgeVector = Camera.main.ViewportToWorldPoint (topRight);
		Vector2 worldBounds = new Vector2 (edgeVector.x * GameController.SCALE, edgeVector.y * GameController.SCALE);
		
		solarSystem.SetWorldBounds (worldBounds);
		contourRenderer.Build ();



		cometPool.PopulatePool (delegate() {
			Body comet = (Body)Instantiate (cometPrefab);
			createdObjects.Add(comet.gameObject);
			return comet.gameObject;
		});
	}



}
