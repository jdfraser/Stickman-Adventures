using UnityEngine;
using System.Collections;

public class Line : SimpleObject {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void pickup(Transform newParent){
		float offset = (collider2D.bounds.size.x / transform.localScale.x * 0.5f); //offset by half the length of the line, accounting for scaling

		base.pickup (newParent);

		transform.Rotate (0, 0, -90);

		if(parent.name == "hand_right"){
			transform.localPosition = new Vector3 (0, -offset, 0);
		}else{
			transform.localPosition = new Vector3 (0, offset, 0);
		}
	}
}
