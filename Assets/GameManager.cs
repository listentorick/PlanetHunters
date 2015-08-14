using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameController gameController;
	public LevelMapController levelMapController;
	public LevelLoader levelLoader;
	private LevelMapItemConfiguration currentLevel;
	public PlayerDataController playerDataController;



	// Use this for initialization
	void Start () {
		levelMapController.LevelSelected+= HandleLevelSelected;
		gameController.Win+= HandleWin;
		playerDataController.Load ();
		LoadLevelMap ();
	}

	public void Next()
	{
		gameController.Reset ();
		LoadLevelMap ();
	}

	void HandleWin ()
	{
		//add scores etc here
		LevelData levelData = new LevelData ();
		levelData.Name = currentLevel.Name;
		levelData.Index = currentLevel.Index;
		levelData.Complete = true;
		playerDataController.LevelCompleted (levelData);
	}

	void HandleLevelSelected (LevelMapItemConfiguration levelDefinition)
	{
		currentLevel = levelDefinition;
		levelMapController.Reset ();
		LoadLevel (levelDefinition.Name);
	}

	public void LoadLevelMap() {
		LevelMap level = levelLoader.LoadLevelMap();
		level.Accept (levelMapController);
		levelMapController.Build ();
	}


	public void LoadLevel(string levelName) {
		Level level = levelLoader.LoadLevel (levelName);
		level.Accept (gameController);
		gameController.BuildLevel ();
	}

	public void ResetLevel() {
		gameController.Reset ();
		LoadLevel (currentLevel.Name);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
