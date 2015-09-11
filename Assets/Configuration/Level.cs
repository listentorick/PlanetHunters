using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

public class Level {


	[XmlElement("Position",typeof(PositionConfiguration))]
	public PositionConfiguration position;


	[XmlAttribute("scale")]
	public float Scale;


	[XmlArray("Bodies")]
	[XmlArrayItem("Planet",typeof(PlanetConfiguration))]
	[XmlArrayItem("Sun",typeof(SunConfiguration))]
	[XmlArrayItem("WormHole",typeof(WormHoleConfiguration))]

	public List<BaseConfiguration> Planets = new List<BaseConfiguration>();

	[XmlArray("Events")]
	[XmlArrayItem("Spawn",typeof(SpawnConfiguration))]
	public List<SpawnConfiguration> Events = new List<SpawnConfiguration> ();

	public void Accept(ILevelConfigurationVisitor visitor) {

		visitor.Visit(this);


		foreach(BaseConfiguration player in Planets) {
			player.Accept(visitor);
		}

		foreach(SpawnConfiguration spawn in Events) {
			spawn.Accept(visitor);
		}

	}
	
}