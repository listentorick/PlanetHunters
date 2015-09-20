using UnityEngine;
using System.Collections;

public class BaseSpawnRequester : MonoBehaviour, IBuild, IReset, IStartStop {

	public delegate void SpawnRequestHandler(Vector2 position, Vector2 velocity);
	public event SpawnRequestHandler SpawnRequest;

	public delegate void SpawningCompleteHandler();
	public event SpawningCompleteHandler SpawningComplete;


	public void OnSpawnRequest(Vector2 pos, Vector2 vel){
		if (SpawnRequest != null) {
			SpawnRequest(pos, vel);
		}
	}

	public void OnSpawningComplete(){
		if (SpawningComplete != null) {
			SpawningComplete();
		}
	}

	public virtual void Build(Ready r)
	{
	}

	public virtual void Reset()
	{
	}

	protected bool stopped = true;

	public void StopPlay()
	{
		stopped = true;
	}
	
	public void StartPlay()
	{
		stopped = false;
	}

	public virtual bool IsComplete(){
		return false;
	}
}
