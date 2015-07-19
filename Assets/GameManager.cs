using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameController gameController;
	public LevelLoader levelLoader;

	// Use this for initialization
	void Start () {

		Level level = levelLoader.LoadLevel ("level1");
		level.Accept (gameController);
		gameController.BuildLevel ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
