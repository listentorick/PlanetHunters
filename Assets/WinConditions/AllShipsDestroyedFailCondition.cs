using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllShipsDestroyedFailCondition : IFailCondition, IBuild {

	public GameController gameController;
	public event FailConditionHandler Fail;
	
	public void Start(){
	}
	
	public void Build (Ready r) {
		gameController.ShipDestroyed += HandleShipCollided;
		r ();
		
	}

	public void Reset(){
		gameController.ShipDestroyed -= HandleShipCollided;
	}

	void HandleShipCollided ()
	{
		if (gameController.GetNumberOfShips () <= 0) {
			Fail();
		}
	}

}
