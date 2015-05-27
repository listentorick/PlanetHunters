using UnityEngine;
using System.Collections;

public class soiChart : MonoBehaviour {

	private Planet planet;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private MeshBuilder meshBuilder;
	private MeshHelper meshHelper;



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetPlanet(Planet planet) {
		this.planet = planet;
		Render ();
	}

	private void Render(){

		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		
		meshBuilder = new MeshBuilder();
		meshHelper = new MeshHelper ();

		meshHelper.BuildDisc (meshBuilder, 0.8f, planet.soi / 100000f, 32, 0, 360);
		
		meshFilter.mesh = meshBuilder.CreateMesh();
		meshRenderer.sortingOrder = 0;
	}
}
