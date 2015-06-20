using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GUIController : MonoBehaviour {

	public GameObject gameOver;
	public GameController gameController;
	public Button replay;
	public Economy economy;
	public Text money;
	public Text numShips;
	public Slider popularity;

	// Use this for initialization
	void Start () {
		ResetControls ();
		gameController.GameOver+= HandleGameOver;
		economy.Profit += HandleProfit;
		gameController.ShipCollided += HandleShipCollided;
		gameController.PopularityChanged += HandlePopularityChanged;
		HandleShipCollided ();
	}

	void HandlePopularityChanged (float pop)
	{
		popularity.value = pop;
	}

	void HandleShipCollided ()
	{
		numShips.text = gameController.GetNumberOfShips ().ToString ();
	}

	void HandleProfit (float profit)
	{
		money.text = economy.playersMoney.ToString();
	}

	private void ResetControls(){
		replay.gameObject.SetActive (false);
		gameOver.SetActive (false);
		money.text = "0";
		numShips.text = "0";
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
