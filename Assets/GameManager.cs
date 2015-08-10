using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameController gameController;
	public LevelMapController levelMapController;
	public LevelLoader levelLoader;
	private string currentLevel = "level1";

	// Use this for initialization
	void Start () {
		levelMapController.LevelSelected+= HandleLevelSelected;
		LoadLevelMap ();
	}

	void HandleLevelSelected (string levelName)
	{
		levelMapController.Reset ();
		LoadLevel (levelName);
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
		LoadLevel (currentLevel);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
