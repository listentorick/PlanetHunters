﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GUIController : MonoBehaviour, IReset, IBuild {

	public GameObject gameOver;
	public GameController gameController;
	public GameManager gameManager;
	public Button replay;
	public Button next;
	public Economy economy;
	public Text money;
	public Text numShips;
	public Slider popularity;
	public PopularityController popularityController;
	public GameObject win;

	// Use this for initialization
	void Start () {
		ResetControls ();
		gameController.GameOver+= HandleGameOver;
		gameController.Win += HandleWin;
		economy.Profit += HandleProfit;
		gameController.ShipCollided += HandleShipCollided;
		popularityController.PopularityChanged += HandlePopularityChanged;
		HandleShipCollided ();
	}



	void HandleWin ()
	{
		win.gameObject.SetActive (true);
		next.gameObject.SetActive (true);
		replay.gameObject.SetActive (true);
	}

	void HandlePopularityChanged (float pop)
	{
		popularity.value = pop;
	}

	void HandleShipCollided ()
	{
		numShips.text = SetText(gameController.GetNumberOfShips ());
	}

	void HandleProfit (float profit)
	{
		//targetScore+= (int)profit;
		//score = Mathf.FloorToInt (economy.playersMoney);
		StartCoroutine (Counter(Mathf.FloorToInt(profit)));
	}

	//private int targetScore;
	private int scoreToRender;

	private void SetMoney(Text text, int value)
	{
		text.text = "$ " + SetText(value);
	}


	private string SetText(int value)
	{
		return value.ToString().Replace("0","o");
	}


	IEnumerator Counter(int count)
	{

		for(int i = 0; i < count/10; i+=10)
		{
			scoreToRender+=i;
			if(scoreToRender>= (int)economy.playersMoney){
				scoreToRender = (int)economy.playersMoney;
			}

			SetMoney(money,scoreToRender);
			yield return null;
		}
	}

	private void ResetControls(){
		replay.gameObject.SetActive (false);
		next.gameObject.SetActive (false);
		gameOver.SetActive (false);
		SetMoney (money,0);
		numShips.text = SetText(0);
		popularity.value = popularityController.popularity;
		win.SetActive (false);
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
		next.gameObject.SetActive (true);
	}

	public void Next() 
	{
		gameManager.Next ();
	}


	public void Replay() 
	{
		gameManager.ResetLevel ();
	}

	public void Reset() 
	{
		ResetControls ();
	}

	public void Build(Ready ready) 
	{
		numShips.text = SetText(gameController.GetNumberOfShips ());
		ready ();
	}

}
