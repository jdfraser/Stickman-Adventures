using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {
	private ArrayList holdables = new ArrayList();


	void OnTriggerEnter2D(Collider2D other){
		Holdable otherHoldable = (Holdable) other.GetComponent (typeof(Holdable));
		holdables.Add (otherHoldable);
		//Debug.Log (otherHoldable.name);
	}
	
	void OnTriggerExit2D(Collider2D other){
		Holdable otherHoldable = (Holdable)other.GetComponent (typeof(Holdable));
		holdables.Remove (otherHoldable);
		//Debug.Log (otherHoldable.name);
	}

	public void grab(ref Holdable currentItem){
		Holdable item;
		
		if(currentItem != null){ //already holding something
			currentItem.drop();
			currentItem = null;
			return;
		}
		
		item = (Holdable) holdables[0];
		
		if(item == null){
			//nothing to pick up
			return;
		}
		
		//passed checks, let's pick something up
		item.pickup (transform);
		currentItem = item;
		
	}
}
