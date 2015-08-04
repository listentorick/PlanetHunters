using UnityEngine;
using System.Collections;

/// <summary>
/// This is the base class for each type of controller
/// These controllers implement the logic of when/how to spawn
/// They request objects are spawned by the CollectablesController
/// </summary>
public abstract class BaseCollectableController : MonoBehaviour, IReset, IBuild {

	public Collectable collectablePrefab; 
	public delegate void SpawnRequestHandler(BaseCollectableController type, Collectable c);
	public event SpawnRequestHandler SpawnRequest;
	public void OnSpawnRequest(Collectable c){
		if (SpawnRequest != null) {
			SpawnRequest(this,c);
		}
	}

	public virtual void Reset(){
		
		
	}

	public virtual void Build(){
		
		
	}




}
