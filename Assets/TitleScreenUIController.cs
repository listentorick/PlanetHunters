using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleScreenUIController : MonoBehaviour {

	public GameManager gameManager;
	public Image title;
	public Button start;
	public Image background;
	public Canvas levelEditor;

	public void EditLevels()
	{
		start.gameObject.SetActive (false);
		title.gameObject.SetActive (false);
		background.gameObject.SetActive(false);
		//levelEditor.gameObject.SetActive (true);
	}


	public void StartGame(){


		//title.CrossFadeAlpha (0, 5f,false);
		start.GetComponentInChildren<Text> ().text = "Loading...";

		gameManager.LoadLevelMap (delegate(LevelMap l) {
		
			StartCoroutine(FadeOut());
		});

	}

	private IEnumerator FadeOut(){
	
		start.GetComponentInChildren<Text> ().CrossFadeAlpha(0f,1f,true);
		title.CrossFadeAlpha(0f,1f,true);
		background.CrossFadeAlpha(0f,1f,true);

		yield return new WaitForSeconds(2);
		
		start.gameObject.SetActive (false);
		title.gameObject.SetActive (false);
		background.gameObject.SetActive(false);

	}


}
