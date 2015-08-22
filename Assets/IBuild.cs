using UnityEngine;
using System.Collections;

public delegate void Ready();

public interface IBuild  {

	// Use this for initialization
	void Build (Ready ready);


}
