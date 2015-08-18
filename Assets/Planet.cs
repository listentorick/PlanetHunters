using UnityEngine;
using System.Collections;

public class Planet : Body {
	
	public soiChart soiChart;
	public ResourceChart resourceChartPrefab;
	private Bounds bounds;
	public Timer timer;
//	public bool castsShadows = true;
	public float imageScale = 1f;

	private Resource[] resourceComponents;
	private ResourceChart[] resourceCharts;

	public int population = 0;

	public Resource[] GetResources(){
		return resourceComponents;
	}

	public void AddPopulation(int delta) {
		this.population += delta;
		Resource pop = GetPopulationResource ();
		if(pop!=null) {
			pop.AddStock(delta);
		}
		SetPopulation (this.population);
		//if (delta < 0) {
			//need to highlight the chart
		//	GetResourceChart (Cargo.People).Highlight(true);
		//} //else {
			//GetResourceChart (Cargo.People).Highlight(false);
		//}
	}

	private Resource GetPopulationResource(){
		return GetResource (Cargo.People);
	}

	public Resource GetResource(Cargo type){
		if (resourceComponents == null)
			return null;
		foreach (Resource r in resourceComponents) {
			//takes 1 second 1 unit
			if(r.resourceType == type){
				return r;
			} 
		}
		return null;
	}

	private ResourceChart GetResourceChart(Cargo type){
		if (resourceCharts == null)
			return null;
		foreach (ResourceChart r in resourceCharts) {
			//takes 1 second 1 unit
			if(r.resourceType == type){
				return r;
			} 
		}
		return null;
	}

	private void SetPopulation(int population) {
	
		//for each resource update the rate of consumption
		foreach (Resource r in resourceComponents) {
			//takes 1 second 1 unit
			if(r.resourceType != Cargo.People){
				r.timeToConsumeOneUnit = 10f / population; 
			} 
		}
	}

	public void SetIsLightSource(bool isLightSource) {
		//if (!castsShadows) {
		if (isLightSource) {
			this.GetComponent<PolygonCollider2D> ().isTrigger = true;
			Destroy (this.GetComponent<PolygonCollider2D> ());
			this.GetComponentInChildren<DynamicLight> ().enabled = true;

		} else {

		}
		//}
	}

	public bool IsLightSource {
		get {return this.GetComponentInChildren<DynamicLight>().enabled;}
	}

	void Start() {
		//bounds = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.bounds;
		//if (!canMove) {
			float xSize = bounds.size.x;
		//	this.transform.GetChild (0).transform.localScale = new Vector3 (2 / xSize, 2 / xSize, 1) * soi / scale;   
		//}
		soiChart.SetPlanet (this);

		timer.TimerEvent+= HandleTimerEvent;
		this.GetRendererTransform().localScale = new Vector3(imageScale,imageScale,imageScale);
		//this.gameObject.layer = 8;



	}

	void HandleTimerEvent ()
	{
		Resource food = this.GetResource (Cargo.Food);
		if (food!=null && food.current <= 0) {
		
			//people gunna die cos no food yo.
			AddPopulation(-1);

		
		}


	}



	public void BuildResourceCharts() {

		resourceComponents = this.GetComponents<Resource> ();
		
		if (resourceComponents.Length == 0)
			return;
		//we need to add a chart for each resource...
		//how many are there?
		resourceCharts = new ResourceChart[resourceComponents.Length ];
		float minAngle = 0;
		float deltaAngle = 360 / resourceComponents.Length;
		for(int i=0; i<resourceComponents.Length;i++) { 
			Resource r = resourceComponents[i];
			r.ResourceLevelChanged+= HandleResourceLevelChanged;
			ResourceChart rC = (ResourceChart)Instantiate(resourceChartPrefab);
			rC.transform.parent = this.transform;
			rC.transform.localPosition = new Vector3(0,0,3f);
			rC.resourceType = r.resourceType;
			resourceCharts[i] =rC;

			rC.Set(r);
			
			rC.minAngle = minAngle;
			rC.maxAngle = rC.minAngle + deltaAngle;
			minAngle+=deltaAngle;
			
			
			
		}
	
	}

	public delegate void ResourceLevelChangedHandler(Resource resource, float value, float delta);
	public event ResourceLevelChangedHandler ResourceLevelChanged;

	void HandleResourceLevelChanged (Resource resource, float value, float delta)
	{
		if (ResourceLevelChanged != null) {
			ResourceLevelChanged(resource,value,delta);
		}
	}

	//void HandleResourceLevelChanged (Cargo type, float value)
//	{


//	}

	void Update () {
		base.Update ();
		Resource food = GetResource (Cargo.Food);
		Resource people = GetResource (Cargo.People);

		if (food!=null && food.current > 0) {
			//we have food so people are no longer dying
			GetResourceChart (Cargo.People).Highlight (false);
		} else if(people!=null){
			//we have food so people are dying
			GetResourceChart (Cargo.People).Highlight(true);
		}
	}



	public delegate void PlanetDeadHandler();
	public event PlanetDeadHandler PlanetDead;

	public void SetSprite(Sprite sprite) 
	{
		this.GetRendererTransform ().GetComponent<SpriteRenderer> ().sprite = sprite;
	}



	public void ConsumeCargo (Ship s) {
	
		//whats the ship got?
		//if (s.cargoType == Cargo.Food) {
			//foodSupplies+= s.cargo;
			//if(foodSupplies>maxFoodSupplies){
		//		foodSupplies = maxFoodSupplies;
		//	}
		//	s.cargo = 0;
		//}
	
	
	
	}


}
