using UnityEngine;
using System.Collections;

public abstract class BaseCollectedHandler : MonoBehaviour {

	public abstract void Collect(Collectable collectable, Ship ship);
}
