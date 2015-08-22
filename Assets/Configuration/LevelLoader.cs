using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;



public class LevelLoader : MonoBehaviour {


	public delegate void LoadLoadedHandler<T>(T data);


	private IEnumerator LoadLevelAsync<T>(string path, LoadLoadedHandler<T> loaded) where T : Level {
		//Debug.Log ("start Load" + Time.time);
		ResourceRequest resourceRequest  = Resources.LoadAsync<TextAsset>(path);
	
		while (!resourceRequest.isDone) {
			//throw events here
		//	Debug.Log ("returning" + Time.time);

			yield return null;
		}

		//Debug.Log ("end Load" + Time.time);

		StringReader strReader = null;
		XmlTextReader xmlFromText = null;
		XmlSerializer serializer = null;
		//T level = null;

		try {
			//Debug.Log ("start serialisation" + Time.time);
		 	strReader = new StringReader(((TextAsset)resourceRequest.asset).text);
		 	xmlFromText = new XmlTextReader(strReader);
			serializer = new XmlSerializer (typeof(T));
			T level = serializer.Deserialize (xmlFromText) as T;
			//Debug.Log ("end serialisation" + Time.time);
			loaded (level);
			//Debug.Log ("end build" + Time.time);

		} finally {
			strReader.Close();
			xmlFromText.Close();
		}



	}
	
	private void Load<T>(string path, LoadLoadedHandler<T> callback) where T : Level {

		StartCoroutine (LoadLevelAsync<T> (path, callback));

	}

	public void  LoadLevelMap(LoadLoadedHandler<LevelMap> callback) {
		this.Load<LevelMap> ("levelMap", callback);
	}


	public void LoadLevel (string levelId, LoadLoadedHandler<Level> callback) {
		this.Load<Level> (levelId.ToString (), callback);
	}
}
