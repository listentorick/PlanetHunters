using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour {


	public Level LoadLevel (string levelId) {

		
		DateTime startTime = System.DateTime.UtcNow;

		Stream stream = null;
		LevelsManifest levelsManifest = null;
		StringReader strReader = null;
		XmlTextReader xmlFromText = null;
		Level level = null;
		try {
			
			XmlSerializer serializer = new XmlSerializer (typeof(Level));
			
			TextAsset textAsset = Resources.Load(levelId.ToString()) as TextAsset;

			strReader = new StringReader(textAsset.text);
			xmlFromText = new XmlTextReader(strReader);
			
			
			level = serializer.Deserialize (xmlFromText) as Level;

		} catch(Exception e) {
			Debug.LogError(e);		
		}
		finally {
			strReader.Close();
			xmlFromText.Close();
		}

		return level;

	}
}
