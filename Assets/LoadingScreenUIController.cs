using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreenUIController : MonoBehaviour {

	public GameManager gameManager;
	public Button loading;
	public Image background;

	private bool isFadingOut;
	private bool isFadingIn;
	private float startTime = 0.0f;
	private float fadeTime = 0.5f;

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive (false);
	}
	

	public void Hide(Ready r) {
		this.gameObject.SetActive (true);
		isFadingOut = true;
		isFadingIn = false;
		startTime = 0f;
		showReady = r;
	}



	private Ready showReady;

	public void Show(Ready r) {
		this.gameObject.SetActive (true);
		//loading.gameObject.SetActive (true);
		//background.gameObject.SetActive(false);
		//background.canvasRenderer.SetAlpha( 0.0f );
		isFadingIn = true;
		isFadingOut = false;
		showReady = r;
		startTime = 0f;

		Color newColor = new Color(background.color.r,background.color.b,background.color.g,0f);
		background.color = newColor;
	}



	void Update()
	{
		if(isFadingIn || isFadingOut)
		{
			float newAlpha = 0;
			float timeRatio = startTime/fadeTime;
			if(isFadingIn) {
				 newAlpha =  Mathf.Lerp(0,1,timeRatio);
			} else {
				 newAlpha =  Mathf.Lerp(1,0,timeRatio);
			}
			startTime += Time.deltaTime;
			//float newAlpha =  Mathf.Lerp(0,1,timeRatio);
			Color newColor = new Color(background.color.r,background.color.b,background.color.g,newAlpha);
			background.color = newColor;
			loading.GetComponentInChildren<Text> ().color = newColor;
			

			
			if(startTime > fadeTime)
			{
				if(isFadingOut)this.gameObject.SetActive (false);
				isFadingIn = false;
				isFadingOut = false;
				startTime = 0f;

				showReady();
			}
		}
	}



}
