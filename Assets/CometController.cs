using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CometController : BodyController { 
	
	public Comet cometPrefab;
	
	public override Body BuildBody(){
		Body comet = (Body)Instantiate (cometPrefab);
		return comet;
	}

	public override Body ConfigureBody(Body b){
		return b;
	}


}
