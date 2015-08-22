using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreenUIController : MonoBehaviour {

	public GameManager gameManager;
	public Button loading;
	public Image background;

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive (false);
	}
	

	public void Hide(Ready r) {
		StartCoroutine (FadeOut(r));
	}

	private IEnumerator FadeOut(Ready r){

		background.CrossFadeAlpha( 0f, 1f, false );
		loading.GetComponentInChildren<Text> ().CrossFadeAlpha(0f,1f,false);

		yield return new WaitForSeconds(2);

		r ();
		
	}

	private Ready showReady;

	public void Show(Ready r) {
		this.gameObject.SetActive (true);
		//loading.gameObject.SetActive (true);
		//background.gameObject.SetActive(false);
		//background.canvasRenderer.SetAlpha( 0.0f );
		isFadingIn = true;
		showReady = r;
	}

	private bool isFadingIn;
	private float startTime = 0.0f;
	private float fadeTime = 1.0f;

	void Update()
	{
		if(isFadingIn)
		{
			float timeRatio = startTime/fadeTime;
			float newAlpha =  Mathf.Lerp(0,1,timeRatio);
			Color newColor = new Color(background.color.r,background.color.b,background.color.g,newAlpha);
			background.color = newColor;
			
			startTime += Time.deltaTime;
			
			if(startTime > fadeTime)
			{
				isFadingIn = false;
				showReady();
			}
		}
	}


	//private IEnumerator FadeIN(Ready r){


		//yield return new WaitForSeconds(2);
		
	//	r ();


		//Color c;
	//	c = background.color;
	//	c.a = 0.5f;
	//	background.color = c;

		//loading.gameObject.SetActive (false);
		//background.canvasRenderer.SetAlpha( 0.0f );
	//	image.CrossFadeAlpha( 1.0f, duration, false );

	//	c = loading.GetComponentInChildren<Text> ().color;
	//	c.a = 0.5f;
	//	loading.GetComponentInChildren<Text> ().color = c;

		//background.gameObject.SetActive(false);
		
//		loading.GetComponentInChildren<Text> ().CrossFadeAlpha(1f,1f,false);
///		background.CrossFadeAlpha(1f,100f,false);
//		
//		yield return new WaitForSeconds(2);
//
	//	r ();

	//}
}
