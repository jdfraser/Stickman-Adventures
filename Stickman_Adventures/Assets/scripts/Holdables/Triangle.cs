using UnityEngine;
using System.Collections;

public class Triangle : SimpleObject {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void pickup(Transform newParent){
		//prevent spikes from killing player on pickup
		if(transform.childCount > 0){
			Transform child = transform.GetChild (0);
			if(child.name == "deathzone"){
				Destroy (child.gameObject);
			}
		}

		float offset = -(collider2D.bounds.size.y / transform.localScale.y * 0.5f); //offset by half the width of the triangle, accounting for scaling

		base.pickup (newParent);

		transform.Rotate (0, 0, -90);

		transform.localPosition = new Vector3 (offset, 0, 0);

	}
}
