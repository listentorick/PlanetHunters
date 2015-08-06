using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameController gameController;
	public LevelLoader levelLoader;
	private string currentLevel = "level1";

	// Use this for initialization
	void Start () {
		LoadLevel (currentLevel);
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
