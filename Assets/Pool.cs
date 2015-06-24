using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour {

	public int poolSize = 20;
	private List<GameObject> pool;

	// Use this for initialization
	void Start () {
		pool = new List<GameObject> ();
	//	for (var i=0; i<poolSize; i++) {
	//		GameObject obj = (GameObject)Instantiate(GameObject);
	//		obj.SetActive(false);
	//		pool.Add(obj);
	//	}
	}

	public delegate GameObject PopulatePoolCallback();

	public void PopulatePool(PopulatePoolCallback callback){ 
		for (var i=0; i<poolSize; i++) {
			GameObject obj = callback();
			obj.SetActive(false);
			pool.Add(obj);
		}
	
	}

	public GameObject GetPooledObject(){
		for (int i=0; i<poolSize; i++) {
			if(!pool[i].activeInHierarchy){
				return pool[i];
			}
		}
		return null;
	}

}
