using UnityEngine;
using System.Collections;

public class ShieldCollectedHandler : BaseCollectedHandler {

	public override void Collect ( Collectable collectable, Ship ship)
	{
		ship.hull = 1f;
	}
}
