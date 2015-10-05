using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteriodController : BodyController { 
	
	public Asteriod asteriodPrefab;
	
	public override Body BuildBody(){
		Body comet = (Body)Instantiate (asteriodPrefab);
		return comet;
	}
	
	public override Body ConfigureBody(Body b){
		return b;
	}
	
	
}
