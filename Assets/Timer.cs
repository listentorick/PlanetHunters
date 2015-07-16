using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float minTime;
	public float maxTime;
	private float elapsedTime;
	private float nextTime;
	public bool startOnAwake = true;
	public bool emitOnAwake = true;
	private bool play = false;

	// Use this for initialization
	void Start () {
		if (startOnAwake) {
			Play();
		}
		//nextTime = Random.Range(minTime,maxTime);
	}
	public void Play() 
	{
		play = true;
		if (emitOnAwake) {
			nextTime = 0f;
		} else {
			nextTime = Random.Range (minTime, maxTime);
		}
	}

	public delegate void TimerEventHandler();
	public event TimerEventHandler TimerEvent;


	// Update is called once per frame
	void Update () {

		if (!play)
			return;

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
