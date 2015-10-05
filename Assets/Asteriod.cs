using UnityEngine;
using System.Collections;

public class Asteriod : Body {

	public Rotates rotates;
	// Update is called once per frame
	void Update () {

		base.Update ();
		//if (canAlign) {
	//		AlignToVector ((position-lastPosition).normalized);
	//	}

	}

	public override void StopPlay()
	{
		base.StopPlay ();
		rotates.StopPlay ();
	}
	
	public override void StartPlay()
	{
		base.StartPlay ();
		rotates.StartPlay ();
	}

}
