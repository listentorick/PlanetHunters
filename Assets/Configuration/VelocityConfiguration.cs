using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class VelocityConfiguration  {

	[XmlAttribute("x")]
	public float X;
	
	[XmlAttribute("y")]
	public float Y;

}
