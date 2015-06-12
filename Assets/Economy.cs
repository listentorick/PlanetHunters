using UnityEngine;
using System.Collections;

public class Economy : MonoBehaviour {

	public SolarSystem solarSystem;

	public float playersMoney = 0f;
	// Use this for initialization
	void Start () {
	
		solarSystem.ShipEnteredOrbit+= HandleShipEnteredOrbit;
	}

	//public float GetPrice(Resource r) {
		//what percentage of the items do we have in stock
	//	return GetBasePrice(r.resourceType) * (r.max - r.current); //10f is the base price
//	}

	//public float CalculateMaxCost(Resource r) {
		//what percentage of the items do we have in stock
	//	return GetBasePrice(r.resourceType) * r.max;	
	//}

	//public float GetBasePrice(Cargo r) {
//		return 10f;
//	}

//	public float GetMaxPrice(Cargo r) {
//		return 10000f;
//	}


	//if there is 1 resource the max price is r.max
	//if there are 10 resources the price would be 100 this is the basePrice

	//gradient x = 10-1 y= r.basePrice - r.maxPrice(value at 10 available)

	//public float GetBasePrice(Cargo type) {
	//	return 10f;
//	}

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

		if (s is TraderShip) {

			for (int i=0; i<resources.Length; i++) {
				if (resources [i].resourceType == ((TraderShip)s).cargoType) {
					float price = GetPrice (resources [i]);
					int space = resources [i].max - resources [i].current;
					int available = ((TraderShip)s).cargo;
					if (available > space) {
						available = space;
					}

					float earnings = available * price;
					playersMoney += earnings;
					Profit (earnings);
					resources [i].AddStock (available);
					((TraderShip)s).cargo -= available;

				}
		
			}

		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
