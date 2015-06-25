﻿using UnityEngine;
using System.Collections;

public class PopularityController : MonoBehaviour {

	public delegate void PopularityChangedHandler(float popularity);
	public event PopularityChangedHandler PopularityChanged;
	public Timer popularityTimer;
	public float popularity;

	public void Start() {
		popularityTimer.TimerEvent+= HandleTimerEvent;
	}

	void HandleTimerEvent ()
	{
		IncrementPopularityBy (0.05f);
	}

	public void IncrementPopularityBy(float delta) {
		this.popularity += delta;
		if (popularity > 1f) {
			popularity = 1f;
		}
		this.PopularityChanged (this.popularity);

	}
}
