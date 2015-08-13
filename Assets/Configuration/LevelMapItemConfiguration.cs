using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class LevelMapItemConfiguration  {

	[XmlAttribute("name")]
	public string Name { get; set; }

	[XmlAttribute("index")]
	public int Index { get; set; }
	
	public virtual void Accept(ILevelConfigurationVisitor visitor) {
		visitor.Visit(this);
	}
}
