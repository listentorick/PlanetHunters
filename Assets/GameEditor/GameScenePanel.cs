using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameScenePanel : MonoBehaviour, IDropHandler, ILevelConfigurationVisitor {

	public int currentWhen = 0;
	private Level currentLevel;

	public GameManager gameManager;
	public ToolsPanelController toolPanelController;
	public GameObjectEditor gameObjectEditorPrefab;
	public LevelSelector levelSelector;

	public Sprite colonyShipSprite;
	public Sprite tradeShipSprite;
	public Sprite cargoShipSprite;
	public Sprite turretSprite;
	public Sprite asteriodSprite;
	public Sprite redPlanetSprite;
	public Sprite bluePlanetSprite;
	public Sprite gasGiantSprite;
	public Sprite sunSprite;
	public Sprite wormHoleSprite;
	public Timeline timeLine;
	public Dictionary<int,List<GameObjectEditor>> whenEditors = new Dictionary<int, List<GameObjectEditor>> (); 
	public SaveDialog saveDialog;
	public RectTransform gameArea;

	public void ToggleEditorVisibility() {
		editorVisible = !editorVisible;
		if (!editorActive)
			editorVisible = false;
		foreach (GameObjectEditor goe in editors) {
			goe.gameObject.SetActive (false);
		}
		if (editorVisible) {

			foreach (GameObjectEditor goe in editors) {
				goe.gameObject.SetActive (true);
			}

			//hide all whens
			foreach(int when in whenEditors.Keys){
				foreach (GameObjectEditor goe in this.whenEditors[when]) {
					goe.gameObject.SetActive (false);
				}
			}
			//show current when
			if(this.whenEditors.ContainsKey(currentWhen)){
				foreach (GameObjectEditor goe in this.whenEditors[currentWhen]) {
					goe.gameObject.SetActive (true);
				}
			}
		}

	}

	private bool editorVisible = true;
	private bool interfaceVisible = true;

	public void ResetInterface() {
		interfaceVisible = false;
		ToggleInterface ();
	}

	public void ToggleInterface(){
		interfaceVisible = !interfaceVisible;
		if (editorActive) {
			toolPanelController.gameObject.SetActive (interfaceVisible);
			timeLine.gameObject.SetActive (interfaceVisible);
			saveDialog.gameObject.SetActive(false);
			levelSelector.gameObject.SetActive (false);
			gameArea.gameObject.SetActive(interfaceVisible);


		} else {
			toolPanelController.gameObject.SetActive (false);
			timeLine.gameObject.SetActive (false);
			saveDialog.gameObject.SetActive(false);
			levelSelector.gameObject.SetActive (false);
			gameArea.gameObject.SetActive(false);
		}

	}

	public void ToggleSaveDialogVisibility() {
	}



	public void Update(){
		if (Input.GetKeyDown (KeyCode.F5))
			Run ();
		if (Input.GetKeyDown (KeyCode.F11))
			//ToggleInterface ();
		if (Input.GetKeyDown (KeyCode.F10))
			ToggleEditorVisibility ();
		if (Input.GetKeyDown (KeyCode.F12)) {
			Edit();
		}
		if (Input.GetKeyDown (KeyCode.F7)) {
			saveDialog.gameObject.SetActive(!saveDialog.gameObject.activeSelf);
		}
		if (Input.GetKeyDown (KeyCode.F6)) {
			levelSelector.gameObject.SetActive(!levelSelector.gameObject.activeSelf);
		}
	}

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

	
	//private float CalculateScale(GameObjectEditor pgoe, Level level) {
//		RectTransform parent = pgoe.gameObject.transform.parent.GetComponent<RectTransform>();
//		return level.Scale/parent.rect.width;
//	}
	
	private void SetPosition(GameObjectEditor pgoe,Level level, PositionConfiguration p){

		//This is the parent
		RectTransform parent = pgoe.gameObject.transform.parent.GetComponent<RectTransform>();



		//the parent is bigger than the visible area (gameArea)
		Rect pgoeRect = pgoe.GetComponent<RectTransform> ().rect;
		Vector2 anchoredPosition = pgoe.GetComponent<RectTransform> ().anchoredPosition;
		Rect gameAreaRect = this.gameArea.GetComponent<RectTransform> ().rect;

		float gameAreaScreenWidth = gameAreaRect.width;
		float gameAreaScreenHeight = gameAreaRect.height;

		float pgoeX = anchoredPosition.x;// + (pgoeRect.width / 2f);
		float pgoeY = anchoredPosition.y;// + (pgoeRect.height / 2f);
				
	
		float offsetX = pgoeX;// - (parent.rect.width/2f);
		float offsetY = pgoeY; //- (parent.rect.height/2f);
		
		float xfactor = level.Scale/gameAreaRect.width;
		
		p.X = offsetX * xfactor * GameController.SCALE ;
		p.Y = offsetY * xfactor * GameController.SCALE ;
	}

	private void Position(GameObjectEditor pgoe,Level level, PositionConfiguration p){
		RectTransform parent = pgoe.gameObject.transform.parent.GetComponent<RectTransform>();
		Rect gameAreaRect = this.gameArea.GetComponent<RectTransform> ().rect;

		//parent.rect.width = width of editor window.  
		// level.scale is number of unity world units on screen.
		//so xfactor converts from editor -> unity coordinates
		//i.e. 30/800 
		float xfactor = level.Scale/gameAreaRect.width; 

		float y = 0;
		float x = 0;

		if (p.from == From.Top) {
			y = gameAreaRect.height/2f;
		} else if (p.from == From.Bottom) {
			y = -gameAreaRect.height/2f;
		} else { 
			y = p.Y/(xfactor * GameController.SCALE);// +  (parent.rect.height/2f);
		}

		if (p.from == From.Left) {
			x = -gameAreaRect.width/2f;
		} else if (p.from == From.Right) {
			x = gameAreaRect.width/2f;
		} else { 
			x = p.X/(xfactor * GameController.SCALE);// +  (parent.rect.width/2f);
		}

		//Debug.Log (p.from + " " + y);

		//float x = p.X / GameController.SCALE;  //converts from universe space -> unity world space
		//x = x / xfactor; //converts to editor space


		//float x = p.X/(xfactor * GameController.SCALE);
		//float y = p.Y/(xfactor * GameController.SCALE);

		//pgoe.transform.position = new Vector2 (x + parent.rect.width/2f - pgoe.GetComponent<RectTransform>().rect.width/2f, y + (parent.rect.height/2f) -  pgoe.GetComponent<RectTransform>().rect.height/2f);
		//pgoe.transform.position = new Vector2 (x, y);
		pgoe.GetComponent<RectTransform>().anchoredPosition = new Vector2 (x, y);

	}
	
	private EditorBuilder BuildSpawnEventEditorDelegate(SpawnType spawnType){
		return delegate() {
			
			GameObjectEditor pgoe = (GameObjectEditor)Instantiate (gameObjectEditorPrefab);
			
			pgoe.editorPanel.AddInput ("when",this.currentWhen.ToString());
			pgoe.editorPanel.AddInput ("vx","0");
			pgoe.editorPanel.AddInput ("vy","0");
			
			AddEditorToTimeline(this.currentWhen,pgoe);
			//timeLine.Build();
			pgoe.SetSprite (GetSpawnSprite(spawnType));
			pgoe.ConfigurationBuilder = delegate(EditorPanel panel, Level level) {
				SpawnConfiguration s = new SpawnConfiguration();
				s.Position = new PositionConfiguration();
				s.Velocity = new VelocityConfiguration(float.Parse(panel.GetValue("vx")),float.Parse(panel.GetValue("vy")));
				s.When =  int.Parse(panel.GetValue("when"));
				level.Events.Add(s);
				
				SetPosition(pgoe,level,s.Position);
				s.SpawnType = spawnType;
				
			};

			pgoe.ConfigurationReader = delegate(EditorPanel panel, BaseConfiguration config) {
				SpawnConfiguration s = (SpawnConfiguration)config;
				//if(prc.ResourceType==Cargo.Food){
				panel.SetValue("when",s.When.ToString());
				panel.SetValue("vx",s.Velocity.X.ToString());
				panel.SetValue("vy",s.Velocity.Y.ToString());


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
			pgoe.editorPanel.AddInput ("soi","1500000");
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

			pgoe.ConfigurationReader = delegate(EditorPanel panel, BaseConfiguration config) {
				PlanetConfiguration p = (PlanetConfiguration)config;
				panel.SetValue("mass",(p.Mass/(1* Mathf.Pow(10,25))).ToString());
				panel.SetValue("soi",p.SOI.ToString());
				foreach(PlanetResourceConfiguration prc in p.Resources){               
					if(prc.ResourceType==Cargo.Food){
						panel.SetValue("food",prc.Current.ToString());
					}
					if(prc.ResourceType==Cargo.People){
						panel.SetValue("people",prc.Current.ToString());
					}
				}
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

			pgoe.ConfigurationReader = delegate(EditorPanel panel, BaseConfiguration config) {
			};

			return pgoe;
		};
		
	}

	private EditorBuilder BuildStartPositionEditorDelegate() {
		return delegate() {
			
			GameObjectEditor pgoe = (GameObjectEditor)Instantiate (gameObjectEditorPrefab);
			
			pgoe.SetSprite (tradeShipSprite);
			//pgoe.transform.localScale = new Vector3(
			pgoe.ConfigurationBuilder = delegate(EditorPanel panel, Level level) {

				level.Position = new PositionConfiguration();
				SetPosition(pgoe,level,level.Position);
				
			};
			
			pgoe.ConfigurationReader = delegate(EditorPanel panel, BaseConfiguration config) {

			};
			
			return pgoe;
		};
	}


	private EditorBuilder BuildWormHoleEditorDelegate() {
		return delegate() {
			
			GameObjectEditor pgoe = (GameObjectEditor)Instantiate (gameObjectEditorPrefab);
			
			pgoe.SetSprite (wormHoleSprite);
			//pgoe.transform.localScale = new Vector3(
			pgoe.ConfigurationBuilder = delegate(EditorPanel panel, Level level) {

				WormHoleConfiguration w = new WormHoleConfiguration();
				level.Planets.Add(w);
				w.Position = new PositionConfiguration();
				SetPosition(pgoe,level,w.Position);
			};
			
			pgoe.ConfigurationReader = delegate(EditorPanel panel, BaseConfiguration config) {
			};
			
			return pgoe;
		};
	}

	public void Start()
	{
		currentLevel = new Level ();
		currentLevel.Scale = 30;
		saveDialog.levelToSave = currentLevel;

		toolPanelController.AddTool (redPlanetSprite,BuildPlanetEditorDelegate(PlanetType.Red));
		toolPanelController.AddTool (bluePlanetSprite,BuildPlanetEditorDelegate(PlanetType.Blue));
		toolPanelController.AddTool (sunSprite,BuildSunEditorDelegate ());
		toolPanelController.AddTool (gasGiantSprite, BuildPlanetEditorDelegate(PlanetType.GasGiant) );
		toolPanelController.AddTool (GetSpawnSprite(SpawnType.CargoShip), BuildSpawnEventEditorDelegate(SpawnType.CargoShip) );
		toolPanelController.AddTool (GetSpawnSprite(SpawnType.ColonyShip), BuildSpawnEventEditorDelegate(SpawnType.ColonyShip) );
		toolPanelController.AddTool (GetSpawnSprite(SpawnType.Turret), BuildSpawnEventEditorDelegate(SpawnType.Turret) );
		toolPanelController.AddTool (GetSpawnSprite(SpawnType.Asteriod), BuildSpawnEventEditorDelegate(SpawnType.Asteriod) );
		toolPanelController.AddTool (tradeShipSprite, BuildStartPositionEditorDelegate() );
		toolPanelController.AddTool (wormHoleSprite, BuildWormHoleEditorDelegate() );

	    timeLine.SlotClicked+= HandleSlotClicked;

		saveDialog.gameObject.SetActive(false);
		levelSelector.gameObject.SetActive (false);




	}


	void DisposeCurrentLevel()
	{
		foreach (int key in this.whenEditors.Keys) {
			foreach(GameObjectEditor goe in this.whenEditors[key]){
				Destroy(goe.gameObject);
			}
		}

		this.whenEditors.Clear ();

		foreach (GameObjectEditor goe in editors) {
			Destroy(goe.gameObject);
		}

		this.editors.Clear ();
	
	}

	void HideAllEvents(){
		foreach (int key in this.whenEditors.Keys) {
			foreach(GameObjectEditor goe in this.whenEditors[key]){
				goe.gameObject.SetActive(false);
			}
		}


	}

	void DebugEditors() {
	
		foreach (int k in this.whenEditors.Keys) {
			Debug.Log(k + " -> " + this.whenEditors[k].Count);
		}
	}

	void HandleSlotClicked (int when)
	{
		HideAllEvents ();
		this.currentWhen = when;

		DebugEditors ();
		if(this.whenEditors.ContainsKey(this.currentWhen)){
		
			foreach(GameObjectEditor goe in this.whenEditors[this.currentWhen])
			{
				goe.gameObject.SetActive(true);
			}
		}
	}

	private bool editorActive = true;

	public void Run()
	{
		editorActive = false;
		Level level = new Level ();
		level.Scale = currentLevel.Scale;
		level.Planets = new List<BaseConfiguration> ();
		level.Events = new List<SpawnConfiguration> ();
		foreach (GameObjectEditor goe in editors) {
			goe.Apply (level);
		}
		
		interfaceVisible = true;
		editorVisible = true;
		ToggleInterface ();
		ToggleEditorVisibility ();
		
		currentLevel = level;
		saveDialog.levelToSave = level;
		
		gameManager.LoadLevelFromConfiguration (level);
	}

	public void Save() {
		ToggleSaveDialogVisibility ();
	}

	public void Edit() {
		if (!editorActive) {
			//the level is running. Kill it with fire
			editorActive = true;
			interfaceVisible = false;
			editorVisible = false;
			ToggleInterface ();
			ToggleEditorVisibility ();
			gameManager.Reset ();	
		}
	}





	#region IDropHandler implementation
	public void OnDrop (PointerEventData eventData)
	{
		Tool tool = eventData.pointerDrag.GetComponent<Tool>();
		GameObjectEditor goe = tool.GetEditor ();

		goe.transform.SetParent (this.transform);
		float scale = Camera.main.GetComponent<CameraFit> ().UnitsForWidth / currentLevel.Scale;
		goe.GetComponent<RectTransform> ().localScale = new Vector2 (scale, scale);
		editors.Add (goe);

		goe.transform.position = eventData.position;

		//Tool.itemBeingDragged.transform.
	}
	#endregion

	public IList<GameObjectEditor> editors = new List<GameObjectEditor>();

	
	public void Visit (Level visitable) {

		DisposeCurrentLevel ();
		currentLevel = visitable;
		saveDialog.levelToSave = visitable;
		GameObjectEditor goe = BuildStartPositionEditorDelegate ()();
		UpdateEditor (goe, visitable);


		ResetInterface ();


	}

	public void Visit (BaseConfiguration visitable) {


	}

	public void Visit (PlanetConfiguration visitable) {

		GameObjectEditor goe = BuildPlanetEditorDelegate (visitable.Type)();
		UpdateEditor (goe, visitable);
	}



	public void Visit (WormHoleConfiguration visitable) {

		GameObjectEditor goe = BuildWormHoleEditorDelegate ()();
		UpdateEditor (goe, visitable);

	}


	public void Visit (SunConfiguration visitable) {
		GameObjectEditor goe = BuildSunEditorDelegate ()();
		UpdateEditor (goe, visitable);
	}
	public void Visit (ConstellationLineConfiguration visitable) {}
	public void Visit (LevelMapItemConfiguration visitable) {}
	public void Visit (PlanetResourceConfiguration visitable) {}
	public void Visit (SpawnConfiguration visitable) {


	


		//horrible hack..
		this.currentWhen = visitable.When;
		GameObjectEditor goe = BuildSpawnEventEditorDelegate (visitable.SpawnType) ();
		UpdateEditor (goe, visitable);
		this.currentWhen = 0;

	}

	private void AddEditorToTimeline(int when, GameObjectEditor goe){
	
		timeLine.AddEvent (when);
		if (!whenEditors.ContainsKey (when)) {
			whenEditors[when] = new List<GameObjectEditor>();
		}
		whenEditors [when].Add (goe);
		goe.gameObject.SetActive (false);
	
	}


	private void UpdateEditor(GameObjectEditor goe, BaseConfiguration config){
		goe.transform.SetParent (this.transform);
		float scale = Camera.main.GetComponent<CameraFit> ().UnitsForWidth / currentLevel.Scale;
		goe.GetComponent<RectTransform> ().localScale = new Vector2 (scale, scale);
		editors.Add (goe);
		Position (goe, currentLevel, config.Position);
		goe.Read (config);
	}

	public void Build()
	{

		timeLine.Build ();
		//make sure all ui is always on top of editors
		saveDialog.gameObject.transform.SetAsLastSibling ();
		levelSelector.gameObject.transform.SetAsLastSibling ();
		timeLine.gameObject.transform.SetAsLastSibling ();
	
	}
}
