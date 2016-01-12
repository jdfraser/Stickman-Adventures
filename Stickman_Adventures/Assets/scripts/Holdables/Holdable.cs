using UnityEngine;
using System.Collections;

public abstract class Holdable : MonoBehaviour {
	public string shape;

	protected Transform parent;

	public abstract void drop ();

	public abstract void pickup (Transform newParent);

	public abstract bool isHeld ();
}
