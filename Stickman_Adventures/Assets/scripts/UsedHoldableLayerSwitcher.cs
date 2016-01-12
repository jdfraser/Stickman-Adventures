using UnityEngine;
using System.Collections;

public class UsedHoldableLayerSwitcher : MonoBehaviour {

	void OnTriggerExit2D(Collider2D other){

		if(other.gameObject.GetComponent<Holdable>()){
			//this is a holdable on the usedHoldables layer. Move it back to the player layer.
			other.gameObject.layer = transform.root.gameObject.layer;
		}

	}
}
