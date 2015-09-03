﻿using UnityEngine;
using System.Collections;

/// <summary>
/// This collectables controller spawns the collectable.
/// It does not control when things are spawned.
/// </summary>
public class CollectablesController : BodyController, IReset, IBuild {

	public Collectable collectablePrefab;
	public CollectableType collectableType;
	
	public override Body BuildBody(){
		Collectable collectable = (Collectable)Instantiate (collectablePrefab);
		collectable.Collected+= HandleCollected;
		return collectable;
	}

	public virtual void HandleCollected (Collectable collectable, Ship ship)
	{
		//
	}
	
	public override Body ConfigureBody (Body b)
	{
		((Collectable)b).type = collectableType;
		return b;
	}

	
}
