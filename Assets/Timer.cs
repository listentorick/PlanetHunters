using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float minTime;
	public float maxTime;
	private float elapsedTime;
	private float nextTime;
	// Use this for initialization
	void Start () {
	
	}

	public delegate void TimerEventHandler();
	public event TimerEventHandler TimerEvent;

	
	// Update is called once per frame
	void Update () {

		if (elapsedTime > nextTime) {
			
			nextTime = Random.Range(minTime,maxTime);
			elapsedTime = 0f;
			
			if(TimerEvent!=null) {
				TimerEvent();
			}
			
			//warped out ships are added back to the pool
		}
		elapsedTime += Time.deltaTime;
	
	}
}
