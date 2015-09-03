using UnityEngine;
using System.Collections;

public class WarpGate : Body, IStartStop {


	public delegate void BodyEnteredWarpGateHandler(Body ship, WarpGate warpGate);
	public event BodyEnteredWarpGateHandler BodyEnteredWarpGate;
	//public Cargo cargoType;
	public Resource resource;
	//public float start = 0f;
	//public float finish = 360f;
	private Vector3 initialScale;
	private Rotates rotatesComponent;
	private bool stop = true;

	public void StopPlay(){
		rotatesComponent.StopPlay ();
		stop = true;
	}

	public void StartPlay(){
		rotatesComponent.StartPlay ();
		stop = false;
	}

	public void Reset(){
		rotatesComponent.Reset ();
		stop = false;
	}

	// Use this for initialization
	void Start () {
		initialScale = this.transform.localScale;

		rotatesComponent = this.GetComponent<Rotates> ();

	}

	public void SetResource(Resource r) {
		resource = r;
		this.GetComponent<SpriteRenderer> ().color = Helpers.GetCargoColor (resource.resourceType);
	}

	//private float lerpTime = 0f;

	// Update is called once per frame
	void Update () {

		base.Update ();
		if (stop) {
			return;
		}
		if (animate) {
			time+=Time.deltaTime * 3f;
			//his.transform.localScale = new Vector3 (1f,1f,);
			if(time<1f){
				float fade = Mathf.Lerp (1f, 0f, time);
				this.transform.localScale = new Vector3 (fade * initialScale.x , fade * initialScale.y, initialScale.z);
			}
			if(time>1)
			{
				//animate = false;
				//time = 0;
				float fade = Mathf.Lerp (0f,1f, time-1f);
				this.transform.localScale = new Vector3 (fade * initialScale.x, fade * initialScale.y, initialScale.z);
				if(time>2){
					animate = false;
					time = 0f;
				}
			}
		
		}
	}
	private float time = 0;
	private bool animate = false;
	public void OnTriggerEnter2D(Collider2D other) {

		Body body = other.gameObject.GetComponent<Body> ();
		if (body != null) {
			animate = true;
			time = 0;
			BodyEnteredWarpGate (body, this);
	
		}
	}
}
