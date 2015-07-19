using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

[XmlRoot("LevelsManifest")]
public class LevelsManifest  {

	[XmlArray("Levels")]
	[XmlArrayItem("Level",typeof(string))]
	public List<string> levels = new List<string>();

	public LevelsManifest() {
	}

}
