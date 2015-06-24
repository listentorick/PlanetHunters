using UnityEngine;
using System.Collections;

public abstract class BaseCollectableController : MonoBehaviour {

	public Collectable collectablePrefab; 
	public delegate void SpawnRequestHandler(BaseCollectableController type, Collectable c);
	public event SpawnRequestHandler SpawnRequest;
	public void OnSpawnRequest(Collectable c){
		if (SpawnRequest != null) {
			SpawnRequest(this,c);
		}
	}


}
