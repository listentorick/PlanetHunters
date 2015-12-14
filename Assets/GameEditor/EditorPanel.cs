
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EditorPanel : MonoBehaviour {


	public RectTransform content;

	//public VerticalLayoutGroup verticalLayoutGroup;
	public InputField inputFieldPrefab;
	public Text textPrefab;

	public Dictionary<string,InputField> map = new Dictionary<string, InputField>();

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddInput(string name, string defaultValue)
	{
		Text label = (Text)Instantiate (textPrefab);
		label.transform.SetParent(content);
		label.text = name;

		InputField inputField = (InputField)Instantiate (inputFieldPrefab);
		inputField.transform.SetParent(content);
		//inputField.transform.localPosition = new Vector3 (0, map.Count * 35, 0);
		inputField.text = defaultValue;
		map.Add (name, inputField);
		content.sizeDelta = new Vector2(content.rect.width,map.Count * 100);
	}

	public string GetValue(string name)
	{
		if (map.ContainsKey (name)) {
			return map[name].text;
		}
		return "";
	}

	public void SetValue(string name, string value)
	{
		//if (map.ContainsKey (name)) {
			map[name].text = value;
		//}

	}
}
