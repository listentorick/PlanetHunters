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
	public bool IsSelected;
	protected float scale = 100000f;





	public void Update () {
	
		this.transform.position = new Vector3(position.x/100000f, position.y/100000f, this.transform.position.z);

	}






}
