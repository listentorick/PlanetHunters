using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System.Xml.Serialization;

public class SaveDialog : MonoBehaviour {

	public ScrollableList scrollableList;
	public InputField filename;
	public Level levelToSave;

	// Use this for initialization
	void Start () {
		scrollableList.OnClicked+= HandleOnClicked;
		List<string> players = new List<string> ();
		string myPath = "Assets/Configuration/Resources/";
		DirectoryInfo dir = new DirectoryInfo (myPath);
		FileInfo[] info = dir.GetFiles ("*.*");
		foreach (FileInfo f in info) {
			if (f.Extension == ".xml") {
				
				scrollableList.AddMenuItem(Path.GetFileNameWithoutExtension(f.Name));
			}
		}

	}

	//private string fn;
	void HandleOnClicked (string text)
	{
	//	fn = text + ".xml";
		filename.text = text;
	}

	public void Save()
	{
		XmlSerializer serializer = null;
		Stream writer = null;

		try {
			string path = @"C:\code\Planet Hunters\Assets\Configuration\Resources\";
		 	serializer =  new XmlSerializer (typeof(Level));
		 	writer = new FileStream(path + filename.text + ".xml", FileMode.Create);
			serializer.Serialize (writer, levelToSave);
		} finally {
			writer.Close ();
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
