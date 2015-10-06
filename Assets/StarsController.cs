using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarsController : MonoBehaviour, IReset, IBuild, IStartStop {
	
	public GameObject starPrefab;
	public Pool pool;
	public bool stop = true;
	private List<IStartStop> stoppables = new List<IStartStop>();


	public void Start(){
			
		pool.PopulatePool (delegate() {
			GameObject star = (GameObject)Instantiate (starPrefab);
			return star;
		});
		

	}

	private void PositionStar(GameObject star){

		float worldScreenHeight = (float)(Camera.main.orthographicSize * 2.0);
		float worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width);

		float x = Random.Range(-worldScreenWidth/2f,worldScreenWidth/2f);
		float y = Random.Range(-worldScreenHeight/2f,worldScreenHeight/2f);
		Vector2 pos = new Vector2(x,y);
		star.transform.position = pos;
		float scale = Random.Range(0.1f,1f);
		star.transform.localScale = new Vector3(scale,scale,scale);
		star.transform.Rotate (Vector3.forward * Random.Range(0f,90f));
	}

	public void Build(Ready ready) {
		for (var i=0; i<pool.poolSize; i++) {
			//while(pool.
			GameObject star = pool.GetPooledObject ();
			stoppables.Add(star.GetComponent<Rotates>());
			if(star){
				star.SetActive (true);
				PositionStar(star);
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
