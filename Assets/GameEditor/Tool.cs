using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public delegate GameObjectEditor EditorBuilder();

public class Tool : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject itemBeingDragged;
	public GameObjectEditor editorPrefab;
	public EditorBuilder EditorBuilder;

	Vector3 startPosition;

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		itemBeingDragged = null;
		transform.position = startPosition;
	}

	#endregion

	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
	}
	#endregion

	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}
	#endregion

	public GameObjectEditor GetEditor(){
		GameObjectEditor goe = EditorBuilder ();
		return goe;
	}


}
