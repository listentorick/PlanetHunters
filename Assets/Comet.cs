﻿using UnityEngine;
using System.Collections;

public class Comet : Body {

	public ParticleSystem thruster;
	// Use this for initialization
	void Start () {
		thruster.Play ();	
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		if (canAlign) {
			AlignToVector ((position-lastPosition).normalized);
		}

	}
}
