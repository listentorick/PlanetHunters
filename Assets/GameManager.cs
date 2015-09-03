using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameController gameController;
	public LevelMapController levelMapController;
	public LevelLoader levelLoader;
	private LevelMapItemConfiguration currentLevel;
	public PlayerDataController playerDataController;
	public TitleScreenUIController titleScreenController;
	public LoadingScreenUIController loadingScreenUIController;



	// Use this for initialization
	void Start () {
		levelMapController.LevelSelected+= HandleLevelSelected;
		gameController.Win+= HandleWin;
		playerDataController.Load ();
		//LoadLevelMap ();
	}

	public void Next()
	{
		loadingScreenUIController.Show (delegate() {
			gameController.Reset ();
			LoadLevelMap (delegate(LevelMap data) {
				levelMapController.StartPlay();
				loadingScreenUIController.Hide (delegate() {

				});
			});
		});
	}

	void HandleWin (WinData winData)
	{
		//add scores etc here
		LevelData levelData = new LevelData ();
		levelData.Complete = true;
		levelData.Index = currentLevel.Index;
		levelData.Name = currentLevel.Name;
		levelData.WinData = winData;
		playerDataController.LevelCompleted (levelData);
	}

	void HandleLevelSelected (LevelMapItemConfiguration levelDefinition)
	{
		currentLevel = levelDefinition;

		LoadLevel (levelDefinition.Name);
	}

	public void LoadLevelMap(LevelLoader.LoadLoadedHandler<LevelMap> handler) {


		levelLoader.LoadLevelMap (delegate(LevelMap l) {
			l.Accept (levelMapController);
			levelMapController.Build (delegate() {
				levelMapController.StartPlay();
				handler(l); //

			});

		});
	}

	public void LoadLevel(string levelName) {
		loadingScreenUIController.Show (delegate() {
			levelMapController.Reset ();
			levelLoader.LoadLevel (levelName, delegate (Level l) {
				l.Accept (gameController);
				gameController.Build (delegate() {
					//gameController.StopPlay();
					gameController.StartPlay();
					loadingScreenUIController.Hide (delegate() {

					});
				});
			});
		});
	}

	public void ResetLevel() {
		gameController.Reset ();
		LoadLevel (currentLevel.Name);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
