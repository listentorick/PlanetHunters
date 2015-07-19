using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class PlanetConfiguration : BaseConfiguration {

	[XmlAttribute("Mass",typeof(float))]
	public float Mass;

	[XmlAttribute("SOI",typeof(float))]
	public float SOI;

	[XmlAttribute("Type",typeof(PlanetType))]
	public PlanetType Type;


	public override void Accept(ILevelConfigurationVisitor visitor) {
		visitor.Visit(this);
	}
}
