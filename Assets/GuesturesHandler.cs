using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuesturesHandler : MonoBehaviour {

	private Ship selectedBody;
	private Camera camera;
	public SolarSystem solarSystem;


	public event SelectHandler SelectionChanged;


	public void Start(){
		camera = (Camera) GameObject.FindObjectOfType(typeof(Camera));
	}

	private Ship GetShipFromSelection(GameObject selection){
		Ship s = selection.GetComponent<Ship> ();
		if (s != null)
			return s;
		return selection.GetComponentInParent<Ship> ();
	}

	private Vector2 WorldPos(Vector2 screenPos){

		float worldScreenHeight = (float)(camera.orthographicSize * 2.0);
		float worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width);

		float newX = ((worldScreenWidth / Screen.width) * screenPos.x) - (0.5f * worldScreenWidth);
		float newY = ((worldScreenHeight / Screen.height) * screenPos.y) -  (0.5f * worldScreenHeight);

		return new Vector2 (newX,newY);

	}

	void OnTap( TapGesture gesture ) 
	{

		List<Body> bodiesToSort = new List<Body> ();
		bodiesToSort.AddRange (solarSystem.bodies);

		bodiesToSort.Sort (delegate(Body x, Body y) {
			float diffx = (new Vector2(x.gameObject.transform.position.x,x.gameObject.transform.position.y) - WorldPos(gesture.Position)).magnitude;
			float diffy = (new Vector2(y.gameObject.transform.position.x,y.gameObject.transform.position.y) - WorldPos(gesture.Position)).magnitude;
			if(diffx<diffy){
				return -1;
			}

			if(diffx>diffy){
				return 1;
			}
			return 0;
		});

		float diff = (new Vector2(bodiesToSort [0].gameObject.transform.position.x,bodiesToSort [0].gameObject.transform.position.y) - WorldPos (gesture.Position)).magnitude;

		if(diff<1){

			Selectable sel = bodiesToSort [0].gameObject.GetComponent<Selectable>();
			sel.OnSelect();

			if(SelectionChanged!=null) SelectionChanged( bodiesToSort [0].gameObject);


			if(bodiesToSort [0].gameObject.GetComponent<TraderShip>()){
				selectedBody = bodiesToSort [0].gameObject.GetComponent<TraderShip>();
				selectedBody.IsSelected = true;
			}

			return;
		}
	
	}
	void OnFingerUp( FingerUpEvent e ) 
	{
		if (selectedBody != null) {
			selectedBody.Thrust(new Vector2(0,0));
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
