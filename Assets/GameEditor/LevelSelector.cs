using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class LevelSelector : MonoBehaviour {//, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public Button menuItemPrefab;
	public Transform content;
	public LevelLoader levelLoader;
	public GameScenePanel gameScenePanel;
	public ScrollableList scrollableList;


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

	/*
	public void Start ()
	{
		List<string> players = new List<string> ();
		string myPath = "Assets/Configuration/Resources/";
		DirectoryInfo dir = new DirectoryInfo (myPath);
		FileInfo[] info = dir.GetFiles ("*.*");
		foreach (FileInfo f in info) {
			if (f.Extension == ".xml") {

				AddMenuItem(Path.GetFileNameWithoutExtension(f.Name));
			}
		}
	}
	
	private void AddMenuItem(string text){
		Button button = Instantiate (menuItemPrefab);
		button.transform.SetParent(content, false);
		button.GetComponentInChildren<Text> ().text = text;
		button.onClick.AddListener(()=> LoadLevel(text));
	}*/

	void HandleOnClicked (string text)
	{
		//	fn = text + ".xml";
		levelLoader.LoadLevel(text,(Level l)=> {l.Accept(gameScenePanel); gameScenePanel.Build();});
	}

	/*
	private void LoadLevel(string text)
	{
		levelLoader.LoadLevel(text,(Level l)=> {l.Accept(gameScenePanel); gameScenePanel.Build();});
	}*/

	// Update is called once per frame
	void Update () {
	
	}

	#region IEndDragHandler implementation
	
	public void OnEndDrag (PointerEventData eventData)
	{
	}
	
	#endregion
	
	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
	}
	#endregion
	
	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}
	#endregion
}
