﻿using UnityEngine;
using System.Collections;

public class CollectablesController : MonoBehaviour, IReset, IBuild {
	
	public ShipSpawner spawner;
	public SolarSystem solarSystem;

	private BaseCollectableController[] controllers;
	void Start () {
		controllers = this.GetComponents<BaseCollectableController> ();
		foreach (BaseCollectableController b in controllers) {
			b.SpawnRequest += HandleSpawnRequest;
		}
	}

	void HandleSpawnRequest (BaseCollectableController type, Collectable c)
	{
		Spawn(c);
	}


	void Spawn(Collectable c){
		c.Collected+= HandleCollected;
		spawner.Spawn (c);
	}
	
	void HandleCollected (Collectable collectable, Ship ship)
	{
		solarSystem.RemoveBody (collectable);


	}

	public void Reset(){
		foreach (BaseCollectableController b in controllers) {
			b.Reset();
		}
	}

	public void Build() {
		foreach (BaseCollectableController b in controllers) {
			b.Build();
		}
	}

}
