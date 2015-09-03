using UnityEngine;
using System.Collections;

public class PopularityController : MonoBehaviour, IReset {

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
		if(this.PopularityChanged!=null) this.PopularityChanged (this.popularity);

	}

	public void Reset()
	{
		popularity = 1f;
	}
}
