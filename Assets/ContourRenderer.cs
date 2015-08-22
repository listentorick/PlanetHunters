using UnityEngine;
using System.Collections;

public abstract class ContourRenderer : MonoBehaviour, IReset, IBuild {
	
	// Update is called once per frame
	public abstract void Build (Ready ready);

	public virtual void Reset(){
	}
}
