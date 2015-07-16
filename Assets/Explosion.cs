using UnityEngine;
using System.Collections;

public class Explosion : Body {
	
	public AudioSource audioSource;
	public AudioClip explosionSound;
	public Timer timer;

	// Use this for initialization
	void Start () {
		//audio.
		timer.TimerEvent += HandleTimerEvent;
		audioSource.PlayOneShot (explosionSound, 1f);
	}

	void HandleTimerEvent ()
	{
		//Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}
}
