using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour {
	
	private T Load<T>(string path) where T : Level {

		Stream stream = null;
		LevelsManifest levelsManifest = null;
		StringReader strReader = null;
		XmlTextReader xmlFromText = null;
		T level = null;
		try {
			
			XmlSerializer serializer = new XmlSerializer (typeof(T));
			
			TextAsset textAsset = Resources.Load(path) as TextAsset;
			
			strReader = new StringReader(textAsset.text);
			xmlFromText = new XmlTextReader(strReader);

			level = serializer.Deserialize (xmlFromText) as T;
			
		} catch(Exception e) {
			Debug.LogError(e);		
		}
		finally {
			strReader.Close();
			xmlFromText.Close();
		}
		
		return level;
	}

	public LevelMap LoadLevelMap() {
		return this.Load<LevelMap> ("levelMap");
	}


	public Level LoadLevel (string levelId) {
		return this.Load<Level> (levelId.ToString ());
	}
}
