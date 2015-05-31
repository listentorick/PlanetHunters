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



	// Use this for initialization
	public float radius = 5;
	public float thickness = 0.1f;

	public Cargo resourceType;
	//public Material chartMa

	public void Set(float s) {
		if (value != s) {
			value = s;
			Render();
		}

	}

	private void Render(){
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();

		meshRenderer.material.color = Helpers.GetCargoColor (resourceType);



		
		meshBuilder = new MeshBuilder();
		meshHelper = new MeshHelper ();
		//value = 0.1f;

		
		meshHelper.BuildDisc (meshBuilder, radius, radius + thickness, 32, minAngle, minAngle + (maxAngle - minAngle) * value);
		
		meshFilter.mesh = meshBuilder.CreateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
