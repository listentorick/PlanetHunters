using UnityEngine;
using System.Collections;

public class ResourceChartBackground : MonoBehaviour {

	public ResourceChart resourceChart;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private MeshBuilder meshBuilder;
	private MeshHelper meshHelper;

	void Start() {
	
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();

		Color c= Helpers.GetCargoColor (resourceChart.resourceType);

		Color newColor = new Color (c.r, c.g, c.b, 1f);

		meshRenderer.material.color = newColor;

		Render ();

	}

	void Render() {

	

		meshBuilder = new MeshBuilder();
		meshHelper = new MeshHelper ();

		meshHelper.BuildDisc (meshBuilder, resourceChart.radius, resourceChart.radius +0.25f, 32, resourceChart.minAngle, resourceChart.maxAngle);
		
		meshFilter.mesh = meshBuilder.CreateMesh();



	}

}
