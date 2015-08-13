using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class PlanetResourceConfiguration {

	[XmlAttribute("Max")]
	public int Max;
	
	[XmlAttribute("Current")]
	public int Current;
	
	[XmlAttribute("Type")]
	public Cargo ResourceType;

	public  void Accept(ILevelConfigurationVisitor visitor) {
		visitor.Visit(this);
	}
}
