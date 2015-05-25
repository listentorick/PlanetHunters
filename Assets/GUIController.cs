using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GUIController : MonoBehaviour {

	public GameObject gameOver;
	public ShipSpawner gameController;
	public Button replay;

	// Use this for initialization
	void Start () {
		ResetControls ();
		gameController.GameOver+= HandleGameOver;
	}

	private void ResetControls(){
		replay.gameObject.SetActive (false);
		gameOver.SetActive (false);
	}

	public void Reset() {
		ResetControls ();
		gameController.Reset ();
	}

	void HandleGameOver ()
	{
		this.GameOver ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void GameOver(){
		replay.gameObject.SetActive (true);
		gameOver.SetActive (true);
	}
}
