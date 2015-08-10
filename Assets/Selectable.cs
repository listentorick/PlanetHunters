using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {


	public void OnSelect()
	{
		if (Select != null) {
			Select(this.gameObject);
		}
	}
	
	public delegate void SelectHandler(GameObject g);
	public event SelectHandler Select;

}
