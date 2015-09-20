using UnityEngine;
using System.Collections;

//this is really a resource container.

public class Resource : MonoBehaviour, IStartStop {

	public Cargo resourceType;
	public int max;
	public int current;
	public float basePrice; //these are functions of the economy really
	public float maxPrice;  //these are functions of the economy really
	
	private float timer;
	public float timeToConsumeOneUnit;

	public delegate void ResourceDepletedHandler(Resource resource, Cargo type);
	public event ResourceDepletedHandler ResourceDepleted;

	public delegate void ResourceLevelChangedHandler(Resource resource, float value, float delta);
	public event ResourceLevelChangedHandler ResourceLevelChanged;
	
	public bool IsFull(){
		return current >= max;
	}

	// Use this for initialization
	void Start () {
		if(ResourceLevelChanged!=null)ResourceLevelChanged(this, current/max,0);
	}

	public void AddStock(int stock) {
		if (stop) {
			return;
		}
		current += stock;
		if (current < 0) {
			current = 0;
		}
		ResourceLevelChanged(this, current/max, stock);
	}

	public void ClearStock(){
		this.AddStock (-current);
	}

	private bool stop = true;
	public void StopPlay() {
		stop = true;
	}

	public void StartPlay() {
		stop = false;
	}

	public void Reset(){
		stop = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (stop)
			return;
		timer += Time.deltaTime;
		if (timer > timeToConsumeOneUnit) {
			timer = 0;
			if(current>0) {
				current-=1;
				if(ResourceLevelChanged!=null) ResourceLevelChanged(this, (float)current/(float)max,-1);
				if(current<=0){
					current = 0;
					ResourceDepleted(this, resourceType);
				}

			}
			
		}
		
		//medicalSuppliesChart.Set(medicalSupplies/maxMedicalSupplies);

	
	}
}
