using UnityEngine;
using System.Collections;

public class ZoneTrigger : MonoBehaviour {
	//public Transform target;//source of the action we'll perform when the player hits us
	public bool executeOnce = true;

	private bool actionExecuted = false;

	void OnTriggerEnter2D(Collider2D other){
		if(executeOnce && actionExecuted){
			//only supposed to execute once, and we already did it. we're done.
			return;
		}

		if(other.transform.root.name == "player"){
			//this is the player. do our action
			Action zoneAction = GetComponent<Action> ();
			actionExecuted = true;
			zoneAction.execute ();
		}
	}
}
