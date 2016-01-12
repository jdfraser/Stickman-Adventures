using UnityEngine;
using System.Collections;

public class ComplexObject : Holdable {

	void Awake(){
	}

	public override void pickup (Transform newParent){
		gameObject.layer = 9; // set to a layer that does not collide with player.
		
		transform.parent = newParent;
		
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	public override void drop(){
		Vector3 worldpos = transform.position;

		if(transform.parent.root.localScale.x < 0){ //player is flipped
			//flip if player was flipped when we were dropped to match player's direction
			transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -transform.eulerAngles.z);
		}

		transform.parent.DetachChildren ();

		transform.localScale = Vector3.one;
		transform.localPosition = worldpos;
	}

	public override bool isHeld(){
		if(transform.parent != null){
			return true;
		}
		
		return false;
	}

	public virtual void use(){

	}

}
