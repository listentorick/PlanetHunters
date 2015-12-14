using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class VelocityConfiguration  {

	public VelocityConfiguration()
	{
	}

	public VelocityConfiguration(float x, float y)
	{
		X = x;
		Y = y;
	}

	[XmlAttribute("x")]
	public float X;
	
	[XmlAttribute("y")]
	public float Y;

}
