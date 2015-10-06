using UnityEngine;
using System.Collections;

public class Rotates : MonoBehaviour, IStartStop {

	public float start = 0f;
	public float finish = 360f;
	public float timeToCompleteRotation = 2f;

	// Use this for initialization
	void Start () {
	
	}

	private bool stop = true;
	public void StopPlay()
	{
		stop = true;
	}

	public void StartPlay()
	{
		stop = false;
	}

	public void Reset()
	{
		stop = true;
		lerpTime = 0;
	}

	private float lerpTime = 0f;

	// Update is called once per frame
	void Update () {
		if (stop) {
			return;
		}
		lerpTime += Time.deltaTime;
		transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(start, finish, lerpTime/timeToCompleteRotation));
		if (lerpTime>=timeToCompleteRotation) { 
			lerpTime = 0;
		}
	}
}
