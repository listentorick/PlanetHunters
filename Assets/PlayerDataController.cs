using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Collections.Generic;


public class PlayerDataController : MonoBehaviour, IReset {

	private PlayerData playerData;
	private List<LevelMapItemConfiguration> levelDefintions = new  List<LevelMapItemConfiguration>();
	private string path;

	void Awake() {
		path = Application.persistentDataPath + "/playerinfo.dat";
	}

	// Use this for initialization
	void Start () {
		//Load ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void Save() {
		BinaryFormatter br = new BinaryFormatter ();
		FileStream file = File.Create (path);
		br.Serialize (file, playerData);
		file.Close ();
	}

	public void Load(){
		if (File.Exists (path)) {
			BinaryFormatter br = new BinaryFormatter ();
			FileStream file = File.Open (path, FileMode.Open);
			playerData = (PlayerData)br.Deserialize (file);
			file.Close ();
		} else {
			playerData = new PlayerData();
			playerData.LevelData = new List<LevelData>();
		}
	}

	private void SortLevelData(){
		playerData.LevelData.Sort (delegate(LevelData x, LevelData y) {
			

			if(x.Index<y.Index){
				return -1;
			} else if(x.Index>y.Index){
				return 1;
			} else{
				return 0;
			}
		});
	}
	private LevelData GetLastCompletedLevel(){
		SortLevelData ();
		LevelData last = null;
		foreach (LevelData l in playerData.LevelData) {
			if(l.Complete){
				last = l;
			}
		}
		return last;
	
	}

	public bool IsLevellocked(LevelMapItemConfiguration levelDefinition){
		LevelData ld = GetLastCompletedLevel ();
		if (ld == null) {
			//no levels are complete
			//only first level should be accessible
			return levelDefinition.Index!=0;
		}
		return levelDefinition.Index > ld.Index + 1;
	}

	public void LevelCompleted(LevelData levelData)
	{
		//foreach (LevelData l in playerData.LevelData) {
		//	if(l.Name == levelData.Name){
		//		//we already know about this level.
		//		return;
		//	}
		//}
		playerData.LevelData.Add (levelData);
		Save ();
	}

	public void AddLevelDefinition(LevelMapItemConfiguration levelMapItem){

		levelDefintions.Add (levelMapItem);
	}

	public void Reset(){
		levelDefintions.Clear ();
	}
}

[Serializable]
public class LevelData
{
	public string Name { get; set; }
	public int Index { get; set; }
	public bool Complete { get; set; }
}

[Serializable]
public class PlayerData 
{
	public List<LevelData> LevelData { get; set; }
}

