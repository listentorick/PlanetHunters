using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class PositionConfiguration {

	[XmlAttribute("x")]
	public float X;
	
	[XmlAttribute("y")]
	public float Y;
	
	[XmlAttribute("z")]
	public float Z;
}
