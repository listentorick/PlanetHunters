using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour, IReset {

	public float minTime;
	public float maxTime;
	private float elapsedTime;
	private float nextTime;
	public bool startOnAwake = true;
	public bool emitOnAwake = true;
	private bool play = false;

	public void Reset(){
		CalculateNextTime ();
	}

	private void CalculateNextTime()
	{
		nextTime = Random.Range (minTime, maxTime);
	}

	// Use this for initialization
	void Start () {
		if (startOnAwake) {
			Play();
		}
	}
	public void Play() 
	{
		play = true;
		if (emitOnAwake) {
			nextTime = 0f;
		} else {
			CalculateNextTime();
		}
	}

	public delegate void TimerEventHandler();
	public event TimerEventHandler TimerEvent;


	// Update is called once per frame
	void Update () {

		if (!play)
			return;

		if (elapsedTime > nextTime) {
			
			CalculateNextTime();
			elapsedTime = 0f;
			
			if(TimerEvent!=null) {
				TimerEvent();
			}
			
			//warped out ships are added back to the pool
		}
		elapsedTime += Time.deltaTime;
	
	}
}
