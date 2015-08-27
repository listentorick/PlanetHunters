using UnityEngine;
using System.Collections;

public class Economy : MonoBehaviour {

	public SolarSystem solarSystem;

	public float playersMoney = 0f;
	// Use this for initialization
	void Start () {
	
		solarSystem.ShipEnteredOrbit+= HandleShipEnteredOrbit;
	}
	
	public float GetMaxPrice(Resource r) {
		
		return r.maxPrice;
		
	}

	public float GetPrice(Resource r) {

		//yaxis is price
		//xaxis is amount

		//base price is an arbitary cost when there are 100 items (planets store 100 items max)
		//max price is the cost when there is only 1 item
		float g = (r.basePrice - r.maxPrice)/(100f-1f);
		float n = r.maxPrice - g; // when 1 item (x=1) y=r.maxPrice so....
		float cost =  g * r.current + n;
		if (cost < 1f) {
			cost = 1f;
		} 
		return cost;
	}


	public int BuyAtBasePrice(Resource r, int volume) {
		float price = r.basePrice * volume;
		int units = Mathf.FloorToInt(playersMoney / r.basePrice);
		if (units > volume) {
			units = volume;
		} 
		r.current = r.current - units; //take the units
		playersMoney -= r.basePrice * units;
		Profit(price);
		return units;

	}

	public delegate void ProfitHandler(float profit);
	public event ProfitHandler Profit;

	public void Reset() {
		playersMoney = 0;
	}

	void HandleShipEnteredOrbit (Body s, Body p)
	{


		Resource[] resources = ((Planet)p).GetResources ();

		if (s is Ship) {

			for (int i=0; i<resources.Length; i++) {
				if (resources [i].resourceType == ((Ship)s).cargoType) {
					float price = GetPrice (resources [i]);
					int space = resources [i].max - resources [i].current;
					int available = ((Ship)s).cargo;
					if (available > space) {
						available = space;
					}

					float earnings = available * price;
					playersMoney += earnings;
					Profit (earnings);
					resources [i].AddStock (available);
					((Ship)s).cargo -= available;

				}
		
			}
		}

		

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
