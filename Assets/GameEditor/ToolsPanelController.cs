using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ToolsPanelController : MonoBehaviour {


	public Tool toolPrefab;
	//public Sprite redPlanetSprite;
	//public Sprite bluePlanetSprite;
	//public Sprite gasGiantSprite;
	//public Sprite sunSprite;
	//public PlanetGameObjectEditor planetEditorPrefab;
	//public GameObjectEditor gameObjectEditorPrefab;

	//public Sprite colonyShipSprite;
	//public Sprite tradeShipSprite;
	//public Sprite cargoShipSprite;
	//public Sprite turretSprite;
	//public Sprite asteriodSprite;

	/*
	private Sprite GetSpawnSprite(SpawnType spawnType) {
		Sprite sprite = null;
		if (spawnType == SpawnType.CargoShip) {
			sprite = cargoShipSprite;
		} else if (spawnType == SpawnType.ColonyShip) {
			sprite = colonyShipSprite;
		} else if (spawnType == SpawnType.Turret) {
			sprite = turretSprite;
		} else if (spawnType == SpawnType.Asteriod) {
			sprite = asteriodSprite;
		} 
		return sprite;
		
	}

	private Sprite GetPlanetSprite(PlanetType planetType) {
		Sprite sprite = null;
		if (planetType == PlanetType.Red) {
			sprite = redPlanetSprite;
		} else if (planetType == PlanetType.Blue) {
			sprite = bluePlanetSprite;
		}else if (planetType == PlanetType.GasGiant) {
			sprite = gasGiantSprite;
		} else {
			sprite = sunSprite;
		}
		return sprite;

	}

	private float CalculateScale(GameObjectEditor pgoe, Level level) {
		RectTransform parent = pgoe.gameObject.transform.parent.GetComponent<RectTransform>();
		return level.Scale/parent.rect.width;
	}

	private void SetPosition(GameObjectEditor pgoe,Level level, PositionConfiguration p){
		RectTransform parent = pgoe.gameObject.transform.parent.GetComponent<RectTransform>();
		float offsetX = (pgoe.transform.position.x - (parent.rect.width/2f));
		float offsetY = (pgoe.transform.position.y - (parent.rect.height/2f));
		
		float xfactor = level.Scale/parent.rect.width;
		
		p.X = offsetX * xfactor * GameController.SCALE ;
		p.Y = offsetY * xfactor * GameController.SCALE ;
	}

	private EditorBuilder BuildSpawnEventEditorDelegate(SpawnType spawnType){
		return delegate() {

			GameObjectEditor pgoe = (GameObjectEditor)Instantiate (gameObjectEditorPrefab);
			
			pgoe.editorPanel.AddInput ("when","0");
			pgoe.editorPanel.AddInput ("vx","0");
			pgoe.editorPanel.AddInput ("vy","0");


			pgoe.SetSprite (GetSpawnSprite(spawnType));
			pgoe.ConfigurationBuilder = delegate(EditorPanel panel, Level level) {
				SpawnConfiguration s = new SpawnConfiguration();
				s.Position = new PositionConfiguration();
				s.Velocity = new VelocityConfiguration(float.Parse(panel.GetValue("vc")),float.Parse(panel.GetValue("vy")));
				s.When =  float.Parse(panel.GetValue("when"));
				level.Events.Add(s);

				SetPosition(pgoe,level,s.Position);
				s.SpawnType = spawnType;
				
			};
			
			
			return pgoe;
		};
	}
	
	private EditorBuilder BuildPlanetEditorDelegate(PlanetType planetType){
		return delegate() {

			Sprite sprite = GetPlanetSprite(planetType);

			//Should instantiate a gameobject editor and then add a PlanetGameObjectEditor
			
			GameObjectEditor pgoe = (GameObjectEditor)Instantiate (gameObjectEditorPrefab);
			
			pgoe.editorPanel.AddInput ("mass","1000");
			pgoe.editorPanel.AddInput ("soi","1000");
			pgoe.editorPanel.AddInput ("people","0");
			pgoe.editorPanel.AddInput ("food","50");

			//CalculateScale(pgoe
			
			pgoe.SetSprite (sprite);
			pgoe.ConfigurationBuilder = delegate(EditorPanel panel, Level level) {
				PlanetConfiguration p = new PlanetConfiguration();
				level.Planets.Add(p);
				float mass = float.Parse(panel.GetValue("mass"));
				p.Mass = mass * Mathf.Pow(10,25);
				p.SOI = float.Parse(panel.GetValue("soi"));
				p.Position = new PositionConfiguration();
				p.Resources = new System.Collections.Generic.List<PlanetResourceConfiguration>();

				PlanetResourceConfiguration people = new PlanetResourceConfiguration();
				people.ResourceType = Cargo.People;
				people.Current = int.Parse(panel.GetValue("people"));
				people.Max = 100;
				p.Resources.Add(people);

				PlanetResourceConfiguration food = new PlanetResourceConfiguration();
				food.ResourceType = Cargo.Food;
				food.Current = int.Parse(panel.GetValue("food"));
				food.Max = 100;
				p.Resources.Add(food);


				SetPosition(pgoe,level,p.Position);
				p.Type = planetType;

			};


			return pgoe;
		};
	
	}


	private EditorBuilder BuildSunEditorDelegate(){
		return delegate() {

			GameObjectEditor pgoe = (GameObjectEditor)Instantiate (gameObjectEditorPrefab);

			pgoe.SetSprite (sunSprite);
			//pgoe.transform.localScale = new Vector3(
			pgoe.ConfigurationBuilder = delegate(EditorPanel panel, Level level) {
				SunConfiguration p = new SunConfiguration();
				level.Planets.Add(p);
				p.Position = new PositionConfiguration();
				SetPosition(pgoe,level,p.Position);
							
			};
			return pgoe;
		};
		
	}

	// Use this for initialization
	void Start () {
		AddTool (redPlanetSprite,BuildPlanetEditorDelegate(PlanetType.Red));
		AddTool (bluePlanetSprite,BuildPlanetEditorDelegate(PlanetType.Blue));
		AddTool (sunSprite,BuildSunEditorDelegate ());
		AddTool (gasGiantSprite, BuildPlanetEditorDelegate(PlanetType.GasGiant) );
		AddTool (GetSpawnSprite(SpawnType.CargoShip), BuildSpawnEventEditorDelegate(SpawnType.CargoShip) );
		AddTool (GetSpawnSprite(SpawnType.ColonyShip), BuildSpawnEventEditorDelegate(SpawnType.ColonyShip) );
		AddTool (GetSpawnSprite(SpawnType.Turret), BuildSpawnEventEditorDelegate(SpawnType.Turret) );
		AddTool (GetSpawnSprite(SpawnType.Asteriod), BuildSpawnEventEditorDelegate(SpawnType.Asteriod) );

	}*/

	public void AddTool(Sprite sprite, EditorBuilder factory) {
		Tool tool = Instantiate(toolPrefab);
		tool.gameObject.transform.parent = this.transform;
		tool.GetComponent<Image> ().sprite = sprite;
		tool.EditorBuilder = factory; 
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
