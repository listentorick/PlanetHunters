using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelMapController : MonoBehaviour, IReset,ILevelConfigurationVisitor, IBuild, IStartStop {

	public SolarSystem solarSystem;
	public const float SCALE = 1000000;
	public Planet planetPrefab;
	public Sprite gasPlanetSprite;
	public Sprite bluePlanetSprite;
	public Sprite redPlanetSprite;
	public Sprite sunSprite;
	public ContourRenderer contourRenderer;
	private List<GameObject> createdObjects = new List<GameObject>();
	public StarsController starController;
	public BackgroundController backgroundController;

	public Line linePrefab;
	public PlayerDataController playerDataController;
	private List<IStartStop> stoppables = new List<IStartStop>();
	public CometController cometController;


	private bool stop = true;
	public void StartPlay()
	{
		stop = false;
		cometController.StartPlay ();
		solarSystem.StartPlay ();
	}

	public void StopPlay()
	{
		stop = true;
		cometController.StopPlay ();
		solarSystem.StopPlay ();
	}



	public void Visit (Level visitable){
		
	}
	
	public void Visit (BaseConfiguration visitable){

	}

	public void Visit (ConstellationLineConfiguration visitable){


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
		Lock l = g.GetComponent<Lock> ();
		if (l != null) {
			return;
		}

		LevelDataRenderer r = g.GetComponent<LevelDataRenderer>();

		//g.GetComponent<LevelCon
		LevelSelected (r.LevelDefinition);
	}

	public delegate void LevelSelectedHandler(LevelMapItemConfiguration level);
	public event LevelSelectedHandler LevelSelected;

	public void Visit (PlanetResourceConfiguration visitable){
	}
	
	public void Visit (WormHoleConfiguration visitable){
	}

	private List<Planet> levels = new List<Planet> ();
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
		
		levels.Add (sun);
		
	}

	public void Reset(){
		solarSystem.Reset ();
		stoppables.Clear ();
		solarSystem.Clear ();
		starController.Reset ();

		contourRenderer.Reset ();
		cometController.Reset ();
		foreach (GameObject g in createdObjects) {
			Destroy(g);
		}

		createdObjects.Clear ();
		levels.Clear ();
	}

	public void Build(Ready r){
		var count = 0;
		Ready done = delegate() {
			count++;
			if(count==4){
				r();
			}
		};

		Vector2 topRight = new Vector2 (1, 1);
		Vector2 edgeVector = Camera.main.ViewportToWorldPoint (topRight);
		Vector2 worldBounds = new Vector2 (edgeVector.x * GameController.SCALE, edgeVector.y * GameController.SCALE);
		
		solarSystem.SetWorldBounds (worldBounds);

		cometController.Build (done);
	
		starController.Build (done);
		contourRenderer.Build (done);
		backgroundController.Build (done);
	}
 	
	public void Visit (LevelMapItemConfiguration visitable){
		Planet sun = levels[visitable.Index];

		//Tell the playerDataController about this level
		playerDataController.AddLevelDefinition (visitable);
	
		if (playerDataController.IsLevellocked (visitable)) {
			sun.gameObject.AddComponent<Lock>();
		}

		LevelDataRenderer r = sun.gameObject.AddComponent<LevelDataRenderer>();
		r.LevelDefinition = visitable;
		//next add the data to the sun so when events fire we can ask questions of it
	}




}
