using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class ConstellationLineConfiguration {
	

	[XmlAttribute("index1")]
	public int index1;
	
	[XmlAttribute("index2")]
	public int index2;
	

	public virtual void Accept(ILevelConfigurationVisitor visitor) {
		visitor.Visit(this);
	}
	
}
