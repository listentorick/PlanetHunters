using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {


	public void OnSelect()
	{
		if (Select != null) {
			Select(this.gameObject);
		}
	}
	

	public event SelectHandler Select;

}

public delegate void SelectHandler(GameObject g);
