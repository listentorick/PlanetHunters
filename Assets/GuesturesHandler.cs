using UnityEngine;
using System.Collections;

public class GuesturesHandler : MonoBehaviour {

	private Body selectedBody;
	private Camera camera;

	public void Start(){
		camera = (Camera) GameObject.FindObjectOfType(typeof(Camera));
	}

	void OnTap( TapGesture gesture ) 
	{
		if (gesture.Selection) {
			if(selectedBody!=null) {
				selectedBody.IsSelected = false;
			}

			selectedBody = gesture.Selection.GetComponent<Body> ();
			selectedBody.IsSelected = true;
			Debug.Log ("Tapped object: " + gesture.Selection.name);
		} else {
			Debug.Log ("No object was tapped at " + gesture.Position + " " + gesture.ElapsedTime);
		}
	}
	void OnFingerUp( FingerUpEvent e ) 
	{
		if (selectedBody != null) {
			selectedBody.additionalForce = new Vector2(0,0);
		}
	}

	void OnFingerDown( FingerDownEvent e ) 
	{
		if (e.Selection == null && selectedBody!=null) {


			//var camera = this.cam<Camera>();
			var p = camera.ScreenToWorldPoint(new Vector3(e.Position.x, e.Position.y, 0));

			var dir = p -selectedBody.transform.position;
			/*
			var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

			if(dir.x<0 && dir.y>0) {
				//topleft
				angle-=90;
			}
			if(dir.x>0  && dir.y>0) {
				angle+=270;
			}
			if(dir.x>0  && dir.y<0) {
				angle-=90;
			}
			if(dir.x<0  && dir.y<0) {
				angle+=270;
			}*/

//			selectedBody.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


			//rotate the ship in the direction we want
			//selectedBody.transform.LookAt(e.Position);
			//selectedBody.thrustersOn = true;
			Debug.Log ("fire thrusters");
			selectedBody.Thrust( dir.normalized * 100000f);
			Debug.Log(selectedBody.additionalForce);
		}
		// time the finger has been held down before being released
		//float elapsedTime = e.;

	}
}
