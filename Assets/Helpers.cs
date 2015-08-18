using UnityEngine;
using System.Collections;

public static class Helpers  {


	// UnityEngine overloads the == opeator for the GameObject type
	// and returns null when the object has been destroyed, but 
	// actually the object is still there but has not been cleaned up yet
	// if we test both we can determine if the object has been destroyed.
	public static bool IsDestroyed(this GameObject gameObject) {
		return gameObject == null && !ReferenceEquals(gameObject, null);
	}

	// Use this for initialization
	public static Color GetCargoColor(Cargo type){ 

		//the resource type impacts the color
		if (type == Cargo.Food) {
			return new Color (145f/255f, 164f/255f, 139f/255f);
		} else if (type == Cargo.Medical) {
			return  new Color (175f/255f, 221f/255f, 233f/255f);
		} else if (type == Cargo.Technology) {
			return  new Color(255f/255f,153f/255f,85f/255f);
		}
		else if (type == Cargo.People) {
			return  new Color(234f/255f,154f/255f,90f/255f);
		}

		return new Color ();

	
	
	}
}
