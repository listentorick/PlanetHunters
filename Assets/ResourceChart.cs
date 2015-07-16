using UnityEngine;
using System.Collections;

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]

public class ResourceChart : MonoBehaviour {

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private MeshBuilder meshBuilder;
	private MeshHelper meshHelper;
	public float value = 0;

	public float minAngle = 0f;
	public float maxAngle = 360f;
	public Economy economy;
	private float maxRadius = 4f;



	// Use this for initialization
	public float radius = 3;
	public float thickness = 0.1f;

	public Cargo resourceType;
	//public Material chartMa
	private Resource resource;

	public void Set(Resource r) {
		resource = r;
		r.ResourceLevelChanged += HandleResourceLevelChanged;
	}

	public Economy GetEconomy() {
		if (economy == null) {
			economy = FindObjectOfType<Economy> ();
		}
		return economy;
	}


	public void Start() {
		economy = FindObjectOfType<Economy> ();
	}
	private bool flash = false;
	private float flashTime = 0f;
	void HandleResourceLevelChanged (Resource r, float value, float change)
	{
		if (change > 0) {
			flashTime = 0f;
			flash = true;
		}
		Render ();
	}

	private void Render(){
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();

		meshRenderer.material.color = Helpers.GetCargoColor (resourceType);



		
		meshBuilder = new MeshBuilder();
		meshHelper = new MeshHelper ();
		value = (float)resource.current/(float)resource.max;

		float maxValue = 1;
	
		float percentageOfResource = (float)resource.current /(float)resource.max;

		float cost = GetEconomy().GetPrice (resource); //10f is the base price
		float maxCost = GetEconomy().GetMaxPrice (resource);

		//float scale = this.transform.s.magnitude;
		//Debug.Log ("scale" + scale);
		float height =cost/maxCost + 0.1f;
		maxRadius = radius + height / 1.25f;
		meshHelper.BuildDisc (meshBuilder, radius , maxRadius , 32, minAngle, minAngle + (maxAngle - minAngle) * percentageOfResource);
		
		meshFilter.mesh = meshBuilder.CreateMesh();
	}

	private bool highlight;

	public void Highlight(bool highlight) {
		this.highlight = highlight;


	}
	
	// Update is called once per frame
	void Update () {
		if (flash == true) {
			flashTime+=Time.deltaTime * 2;
			meshRenderer.material.color =  Color.Lerp(  Helpers.GetCargoColor (resourceType),Color.white,flashTime);
			if (flashTime>1f) {
				meshRenderer.material.color =  Color.Lerp( Color.white,Helpers.GetCargoColor (resourceType),flashTime -1);
				if(flashTime>2f){
					flash = false;
				}
			}
		} else {

			if (highlight == true) {
				float lerp = Mathf.PingPong (Time.time, 1.0f) / 1.0f;
				meshRenderer.material.color = Color.Lerp (Helpers.GetCargoColor (resourceType), new Color (1, 0, 0), lerp);
			} else {
				meshRenderer.material.color = Helpers.GetCargoColor (resourceType);
			}
		}

	}
}
