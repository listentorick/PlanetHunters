using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelMap : Level{
	
	public void Accept(ILevelConfigurationVisitor visitor) {
		
		visitor.Visit(this);
		
		foreach(BaseConfiguration player in Planets) {
			player.Accept(visitor);
		}
		
	}

}
