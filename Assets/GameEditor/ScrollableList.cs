using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ScrollableList : MonoBehaviour {

	public Button menuItemPrefab;
	public Transform content;
	public delegate void ClickAction(string text);

	public event ClickAction OnClicked;

	public void AddMenuItem(string text){
		Button button = Instantiate (menuItemPrefab);
		button.transform.SetParent(content, false);
		button.GetComponentInChildren<Text> ().text = text;
		button.onClick.AddListener(()=> OnClicked(text));
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
