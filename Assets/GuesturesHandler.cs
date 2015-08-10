using UnityEngine;
using System.Collections;

public class GuesturesHandler : MonoBehaviour {

	private Ship selectedBody;
	private Camera camera;

	public void Start(){
		camera = (Camera) GameObject.FindObjectOfType(typeof(Camera));
	}

	private Ship GetShipFromSelection(GameObject selection){
		Ship s = selection.GetComponent<Ship> ();
		if (s != null)
			return s;
		return selection.GetComponentInParent<Ship> ();
	}

	void OnTap( TapGesture gesture ) 
	{
		if (gesture.Selection) {

			Ship s = GetShipFromSelection(gesture.Selection);
			Selectable sel = gesture.Selection.GetComponent<Selectable>();

			if(s){

				if(selectedBody!=null ) {
					selectedBody.IsSelected = false;
				}

				selectedBody = s;
				selectedBody.IsSelected = true;
			} else if(sel)
			{
				sel.OnSelect();
			}


			Debug.Log ("Tapped object: " + gesture.Selection.name);
		} else {
			Debug.Log ("No object was tapped at " + gesture.Position + " " + gesture.ElapsedTime);
		}
	}
	void OnFingerUp( FingerUpEvent e ) 
	{
		if (selectedBody != null) {
			selectedBody.Thrust(new Vector2(0,0));
			//selectedBody.additionalForce = new Vector2(0,0);
		}
	}

	void OnFingerDown( FingerDownEvent e ) 
	{
		//if the finger hasnt hit anything 
		//and not a ship
		//and there is a selected body
		if ((e.Selection == null || GetShipFromSelection(e.Selection)==null) && selectedBody!=null) {

			var p = camera.ScreenToWorldPoint(new Vector3(e.Position.x, e.Position.y, 0));

			var dir = p -selectedBody.transform.position;
		
			selectedBody.Thrust( dir.normalized * 50000f);

		}


	}
}
