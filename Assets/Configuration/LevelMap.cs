using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

public class LevelMap : Level{



	[XmlArray("Lines")]
	public List<ConstellationLineConfiguration> Lines = new List<ConstellationLineConfiguration>();

	public void Accept(ILevelConfigurationVisitor visitor) {
		
		visitor.Visit(this);
		
		foreach(BaseConfiguration player in Planets) {
			player.Accept(visitor);
		}

		foreach (ConstellationLineConfiguration l in Lines) {
			l.Accept(visitor);
		}
		
	}

}
