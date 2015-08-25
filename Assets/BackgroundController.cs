using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour, IBuild {

	// Use this for initialization
	public void Build (Ready r) {


		Helpers.ResizeSpriteToScreen (this.gameObject, Camera.main, 1, 0);
	
		r ();
	}

}
