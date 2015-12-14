using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void ConfigurationBuilder(EditorPanel panel, Level level);
public delegate void ConfigurationReader(EditorPanel panel, BaseConfiguration level);

//This exposes the data required for serialisation
public class GameObjectEditor :  MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler {


	public InputField textboxPrefab;
	public EditorPanel editorPanel;
	public Image image;

	public void SetSprite(Sprite sprite){
		image.sprite = sprite;
	}

	public void Apply(Level level){
		ConfigurationBuilder (editorPanel, level);
	}

	public void Read(BaseConfiguration config){
		ConfigurationReader (editorPanel, config);
	}

	public ConfigurationBuilder ConfigurationBuilder; 
	public ConfigurationReader ConfigurationReader; 



	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		//throw new System.NotImplementedException ();
	}

	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}

	#endregion

	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{
		//throw new System.NotImplementedException ();
	}

	#endregion



}
