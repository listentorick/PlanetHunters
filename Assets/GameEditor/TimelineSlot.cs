using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimelineSlot : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Fill(){
		this.GetComponent<Image> ().color = Color.red;
	}

	public void Empty(){
		this.GetComponent<Image> ().color = Color.white;
	}
}
