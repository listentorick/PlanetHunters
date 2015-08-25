using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour, IReset {

	public int poolSize = 20;
	private List<GameObject> pool = new List<GameObject>();

	// Use this for initialization
	void Start () {

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

	public void ReturnAll(){
		foreach (GameObject g in pool) {
			g.SetActive(false);	
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

	public void Reset() 
	{
		foreach(GameObject go in pool){
			Destroy(go);
		}
		pool.Clear();
	}

}
