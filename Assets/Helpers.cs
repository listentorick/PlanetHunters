using UnityEngine;
using System.Collections;

public static class Helpers  {

	// Use this for initialization
	public static Color GetCargoColor(Cargo type){ 

		//the resource type impacts the color
		if (type == Cargo.Food) {
			return new Color (85f/255f, 255f/255f, 85f/255f);
		} else if (type == Cargo.Medical) {
			return  new Color (175f/255f, 221f/255f, 233f/255f);
		} else if (type == Cargo.Technology) {
			return  new Color(255f/255f,153f/255f,85f/255f);
		}
		else if (type == Cargo.People) {
			return  new Color(255f/255f,153f/255f,85f/255f);
		}

		return new Color ();

	
	
	}
}
