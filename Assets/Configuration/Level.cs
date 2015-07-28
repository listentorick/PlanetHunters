﻿using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

public class Level {


	[XmlArray("Bodies")]
	[XmlArrayItem("Planet",typeof(PlanetConfiguration))]
	[XmlArrayItem("Sun",typeof(SunConfiguration))]
	[XmlArrayItem("WormHole",typeof(WormHoleConfiguration))]

	public List<BaseConfiguration> Planets = new List<BaseConfiguration>();

	public void Accept(ILevelConfigurationVisitor visitor) {

		visitor.Visit(this);
		
		foreach(BaseConfiguration player in Planets) {
			player.Accept(visitor);
		}

	}
	
}