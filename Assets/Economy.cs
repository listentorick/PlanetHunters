using UnityEngine;
using System.Collections;

public class Economy : MonoBehaviour {

	public SolarSystem solarSystem;

	public float playersMoney = 0f;
	// Use this for initialization
	void Start () {
	
		solarSystem.ShipEnteredOrbit+= HandleShipEnteredOrbit;
	}

	public float GetPrice(Resource r) {
		//what percentage of the items do we have in stock
		return (r.max / r.current); //the more we have the cheaper
		
	}

	public delegate void ProfitHandler(float profit);
	public event ProfitHandler Profit;

	public void Reset() {
		playersMoney = 0;
	}

	void HandleShipEnteredOrbit (Body s, Body p)
	{


		Resource[] resources = ((Planet)p).GetResources ();

		for(int i=0; i<resources.Length;i++){
			if(resources[i].resourceType == ((Ship)s).cargoType){
				float price = GetPrice(resources[i]);
				int space = resources[i].max - resources[i].current;
				int available = ((Ship)s).cargo;
				if(available>space) {
					available = space;
				}

				float earnings = available * price;
				playersMoney+=earnings;
				Profit(earnings);
				resources[i].AddStock(available);
				((Ship)s).cargo-=available;

			}
		
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
