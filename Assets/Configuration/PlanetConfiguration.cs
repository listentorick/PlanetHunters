using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class PlanetConfiguration : BaseConfiguration {

	[XmlAttribute("Mass",typeof(float))]
	public float Mass;

	[XmlAttribute("SOI",typeof(float))]
	public float SOI;

	[XmlAttribute("Type",typeof(PlanetType))]
	public PlanetType Type;
	
	[XmlArrayItem("Resource",typeof(PlanetResourceConfiguration))]
	public List<PlanetResourceConfiguration> Resources = new List<PlanetResourceConfiguration>();
	
	public override void Accept(ILevelConfigurationVisitor visitor) {
		visitor.Visit(this);
		foreach (PlanetResourceConfiguration r in Resources) {
			r.Accept(visitor);
		}
	}
}
