using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI;

public class PlayerDataGUIController : MonoBehaviour {


	public Text score;
	//public Text scoreTitle;
	public Text name;

	public void SetPlayerData(List<LevelData> playerData){

		if (playerData.Count > 0) {
			if (playerData [0].WinData != null) {
				score.text = playerData [0].WinData.Score.ToString ();
			} else {
				score.text = "ooooo";
				//scoreTitle.enabled = false;
			}
			name.text = playerData [0].Name;
		}
	}

}
