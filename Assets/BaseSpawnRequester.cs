using UnityEngine;
using System.Collections;

public class BaseSpawnRequester : MonoBehaviour, IBuild, IReset {

	public delegate void SpawnRequestHandler(Vector2 position, Vector2 velocity);
	public event SpawnRequestHandler SpawnRequest;

	public void OnSpawnRequest(Vector2 pos, Vector2 vel){
		if (SpawnRequest != null) {
			SpawnRequest(pos, vel);
		}
	}

	public virtual void Build(Ready r)
	{
	}

	public virtual void Reset()
	{
	}
}
