using UnityEngine;
using System.Collections;

public class PowerCollectedHandler : BaseCollectedHandler {

	public override void Collect ( Collectable collectable, Ship ship)
	{
		ship.fuel = 1f;
	}
}
