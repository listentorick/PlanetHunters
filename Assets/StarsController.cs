using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarsController : MonoBehaviour, IReset, IBuild, IStartStop {
	
	public GameObject starPrefab;
	public Pool pool;
	public bool stop = true;
	private List<IStartStop> stoppables = new List<IStartStop>();


	public void Start(){
		float worldScreenHeight = (float)(Camera.main.orthographicSize * 2.0);
		float worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width);
		
		pool.PopulatePool (delegate() {
			
			float x = Random.Range(-worldScreenWidth/2f,worldScreenWidth/2f);
			float y = Random.Range(-worldScreenHeight/2f,worldScreenHeight/2f);
			Vector2 pos = new Vector2(x,y);
			GameObject star = (GameObject)Instantiate (starPrefab,pos,Quaternion.identity);
			float scale = Random.Range(0.1f,1f);
			star.transform.localScale = new Vector3(scale,scale,scale);
			star.transform.Rotate (Vector3.forward * Random.Range(0f,90f));
			return star;
		});
		

	}

	public void Build(Ready ready) {
		for (var i=0; i<pool.poolSize; i++) {
			//while(pool.
			GameObject star = pool.GetPooledObject ();
			if(star){
				star.SetActive (true);
			}
		}
		ready ();
	}
	
	public void Reset(){
		stoppables.Clear ();
		pool.ReturnAll ();
		stop = true;
	}
	
	public void StartPlay(){
		stop = false;
		foreach (IStartStop s in stoppables) {
			s.StartPlay();
		}
	}
	
	public void StopPlay()
	{
		stop = true;
		foreach (IStartStop s in stoppables) {
			s.StopPlay();
		}
	}
	

}
