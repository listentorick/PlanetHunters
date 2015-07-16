using UnityEngine;
using System.Collections;

public class WarpGate : MonoBehaviour {


	public delegate void ShipEnteredWarpGateHandler(Ship ship, WarpGate warpGate);
	public event ShipEnteredWarpGateHandler ShipEnteredWarpGate;
	//public Cargo cargoType;
	public Resource resource;
	//public float start = 0f;
	//public float finish = 360f;
	private Vector3 initialScale;

	// Use this for initialization
	void Start () {
		initialScale = this.transform.localScale;

	}

	public void SetResource(Resource r) {
		resource = r;
		this.GetComponent<SpriteRenderer> ().color = Helpers.GetCargoColor (resource.resourceType);
	}

	//private float lerpTime = 0f;

	// Update is called once per frame
	void Update () {
	//	lerpTime += (Time.deltaTime)/2f;
	//	transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(start, finish, lerpTime));
	//	if (lerpTime>=1f) { 
	//		lerpTime = 0;
	//	}
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
		animate = true;
		time = 0;
		Ship ship = other.gameObject.GetComponent<Ship>();
		if(ship!=null) ShipEnteredWarpGate(ship, this);
	}
	
}
