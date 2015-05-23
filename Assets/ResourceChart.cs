using UnityEngine;
using System.Collections;

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]

public class ResourceChart : MonoBehaviour {

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private MeshBuilder meshBuilder;
	private MeshHelper meshHelper;
	private float value = 0;



	// Use this for initialization
	public float radius = 5;
	public float thickness = 1;

	void Start () {


	
	}

	public void Set(float s) {
		if (value != s) {
			value = s;
			Render();
		}

	}

	private void Render(){
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		
		meshBuilder = new MeshBuilder();
		meshHelper = new MeshHelper ();
		
		meshHelper.BuildDisc (meshBuilder, radius, radius + thickness, 32, 360f * value);
		
		meshFilter.mesh = meshBuilder.CreateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
