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
	public float maxRadius = 6f;



	// Use this for initialization
	public float radius = 5;
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

	void HandleResourceLevelChanged (Cargo type, float value)
	{
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

		float height =cost/maxCost + 0.1f;
		maxRadius = radius + height / 1.25f;
		meshHelper.BuildDisc (meshBuilder, radius, maxRadius, 32, minAngle, minAngle + (maxAngle - minAngle) * percentageOfResource);
		
		meshFilter.mesh = meshBuilder.CreateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
