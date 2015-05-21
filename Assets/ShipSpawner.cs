using UnityEngine;
using System.Collections;

public class ShipSpawner : MonoBehaviour {

	public Body shipPrefab;
	private float elaspedTime = 0;
	private float nextTime = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (elaspedTime > nextTime) {
			nextTime = Random.Range(10,30);
			elaspedTime = 0f;
			Body newShip = Instantiate(shipPrefab);
			newShip.position = new Vector2(Random.Range(-10000,10000),Random.Range(-10000,10000));
		}
		elaspedTime += Time.deltaTime;
	}
}
