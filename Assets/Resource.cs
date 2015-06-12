using UnityEngine;
using System.Collections;

//this is really a resource container.

public class Resource : MonoBehaviour {

	public Cargo resourceType;
	public int max;
	public int current;
	public float basePrice; //these are functions of the economy really
	public float maxPrice;  //these are functions of the economy really
	
	private float timer;
	public float timeToConsumeOneUnit;

	public delegate void ResourceDepletedHandler(Cargo type);
	public event ResourceDepletedHandler ResourceDepleted;

	public delegate void ResourceLevelChangedHandler(Cargo type, float value);
	public event ResourceLevelChangedHandler ResourceLevelChanged;
	


	// Use this for initialization
	void Start () {
		if(ResourceLevelChanged!=null)ResourceLevelChanged(resourceType, current/max);
	}

	public void AddStock(int stock) {
		current += stock;
		ResourceLevelChanged(resourceType, current/max);
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;
		if (timer > timeToConsumeOneUnit) {
			timer = 0;
			current-=1;
			if(ResourceLevelChanged!=null) ResourceLevelChanged(resourceType, (float)current/(float)max);
			if(current<=0){
				ResourceDepleted(resourceType);
				current = 0;
			}
			
		}
		
		//medicalSuppliesChart.Set(medicalSupplies/maxMedicalSupplies);

	
	}
}
