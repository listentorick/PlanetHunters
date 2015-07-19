using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class LevelsManifestLoader: MonoBehaviour
{
	public LevelsManifest LoadLevelsManifest () {
		
		Stream stream = null;
		LevelsManifest levelsManifest = null;
		StringReader strReader = null;
		XmlTextReader xmlFromText = null;

		try {
			
			XmlSerializer serializer = new XmlSerializer (typeof(LevelsManifest));

			TextAsset textAsset = Resources.Load("manifest") as TextAsset;

			strReader = new StringReader(textAsset.text);
			xmlFromText = new XmlTextReader(strReader);


			levelsManifest = serializer.Deserialize (xmlFromText) as LevelsManifest;
			
		} catch(Exception e) {
			Debug.LogError(e);		
		}
		finally {
			strReader.Close();
			xmlFromText.Close();
		}

		return levelsManifest;
		
	}
	
}


