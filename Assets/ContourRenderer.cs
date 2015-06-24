using UnityEngine;
using System.Collections;

public abstract class ContourRenderer : MonoBehaviour, IReset {
	
	// Update is called once per frame
	public abstract void Build ();

	public virtual void Reset(){
	}
}
