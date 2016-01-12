using UnityEngine;
using System.Collections;

public class Circle : SimpleObject {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void pickup(Transform newParent){
		float offset = -(collider2D.bounds.size.x / transform.localScale.x * 0.5f); //offset by half the width of the circle, accounting for scaling
		base.pickup (newParent);

		transform.localPosition = new Vector3 (offset, 0, 0);
	}
}
