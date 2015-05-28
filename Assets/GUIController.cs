﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GUIController : MonoBehaviour {

	public GameObject gameOver;
	public ShipSpawner gameController;
	public Button replay;
	public Economy economy;
	public Text money;

	// Use this for initialization
	void Start () {
		ResetControls ();
		gameController.GameOver+= HandleGameOver;
		economy.Profit += HandleProfit;
	}

	void HandleProfit (float profit)
	{
		money.text = economy.playersMoney.ToString();
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
