using UnityEngine;
using System.Collections;

public class Body : MonoBehaviour {

	public ParticleSystem thruster;

	public Vector2 position;
	public Vector2 velocity;
	public Vector2 acceleration;
	public float mass;
	public bool canMove;
	public bool inOrbit;
	public bool justEnteredOrbit;
	public Body parentBody;
	public Vector2 additionalForce = new Vector2(0,0);
	public float soi;
	public float fuel = 1f;
	public float burnRate = 0.01f;
	private Bounds bounds;
	private Transform shipRendererTransform;
	private Renderer[] renderers;
	public bool IsSelected;

	public void Start(){
		SolarSystem sol = GameObject.FindObjectOfType<SolarSystem> ();
		sol.AddBody (this);

		bounds = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.bounds;
		if (!canMove) {
			float xSize = bounds.size.x;
			this.transform.GetChild (0).transform.localScale = new Vector3 (2 / xSize, 2 / xSize, 1) * soi / scale;   
		}

		shipRendererTransform = this.gameObject.transform.GetChild (0);

		renderers = this.GetComponentsInChildren<Renderer> ();
	}



	public void Thrust(Vector2 thrust){
		if (fuel <= 0)
			return;
		additionalForce = thrust;
	}
	private float rechargeTime = 0;
	public void Update () {


		if (additionalForce != Vector2.zero && canMove) {
			fuel -= burnRate;
			thruster.Play();
			if(fuel<0){
				fuel = 0;
				rechargeTime =0;
			}

			var angle = Mathf.Atan2 (additionalForce.y, additionalForce.x) * Mathf.Rad2Deg;
			
			if (additionalForce.x < 0 && additionalForce.y > 0) {
				//topleft
				angle -= 90;
			}
			if (additionalForce.x > 0 && additionalForce.y > 0) {
				angle += 270;
			}
			if (additionalForce.x > 0 && additionalForce.y < 0) {
				angle -= 90;
			}
			if (additionalForce.x < 0 && additionalForce.y < 0) {
				angle += 270;
			}
			
			shipRendererTransform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

		} else {
		
			if(thruster!=null)thruster.Stop();
			if(rechargeTime>5f){ 
				fuel +=  Time.deltaTime/10;
				if(fuel>1){
					fuel = 1;
				}
			}

		}

		this.transform.position = new Vector2(position.x/100000f, position.y/100000f);

		ScreenWrap ();
		rechargeTime += Time.deltaTime;
	}

	private float scale = 100000f;


	private bool isWrappingX;
	private bool isWrappingY;

	private bool IsRendererVisible() {
		foreach (Renderer r in renderers) {
			if(r.isVisible==true){
				return true;
			}
		}
		return false;
	}

	void ScreenWrap()
	{

		
		if(IsRendererVisible())
		{
			isWrappingX = false;
			isWrappingY = false;
			return;
		}
		
		if(isWrappingX && isWrappingY) {
			return;
		}
		
		var cam = Camera.main;
		var viewportPosition = cam.WorldToViewportPoint(transform.position);
		var newPosition = transform.position;
		
		if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
		{
			newPosition.x = -newPosition.x;
			
			isWrappingX = true;
		}
		
		if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
		{
			newPosition.y = -newPosition.y;
			
			isWrappingY = true;
		}

		position = new Vector2(newPosition.x,newPosition.y) * scale;
		
		this.transform.position = newPosition;
	}
}
