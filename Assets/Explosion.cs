﻿using UnityEngine;
using System.Collections;

public class Explosion : Body  {
	
	public AudioSource audioSource;
	public AudioClip explosionSound;

	// Use this for initialization
	void Start () {
		audioSource.PlayOneShot (explosionSound, 1f);
	}

	//public override void StopPlay()
	//{
	//	stop = false;  //dont stop we want
	//}


}
