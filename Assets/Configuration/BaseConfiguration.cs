using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class BaseConfiguration {
	
	[XmlElement("Position",typeof(PositionConfiguration))]
	public PositionConfiguration Position;

	public virtual void Accept(ILevelConfigurationVisitor visitor) {
		visitor.Visit(this);
	}

}
