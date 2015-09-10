using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;


public class SpawnConfiguration : BaseConfiguration {

	
	[XmlAttribute("when")]
	public float When;
	
	[XmlAttribute("type")]
	public SpawnType SpawnType;

	[XmlElement("Velocity",typeof(VelocityConfiguration))]
	public VelocityConfiguration Velocity;

	public override void Accept(ILevelConfigurationVisitor visitor) {
		visitor.Visit(this);
	}

}
