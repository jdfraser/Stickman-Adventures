using UnityEngine;
using System;
using System.Collections;

public class PickupRange : MonoBehaviour {
	private const float PICKUP_OFFSET = 2.7f;
	private const float DEFAULT_Y_OFFSET = -1.3f;
	private const float DOWN_OFFSET = 2f;

	CircleCollider2D circle;
	private PlayerController player;
	// Use this for initialization
	void Start () {
		player = (PlayerController) FindObjectOfType (typeof(PlayerController));
		circle = (CircleCollider2D) collider2D;
	}

	void Update(){

		if(Input.GetKey (KeyCode.S)){
			circle.center = new Vector2(0, -PICKUP_OFFSET - DOWN_OFFSET);
		}else if(Input.GetKey (KeyCode.W)){
			circle.center = new Vector2(0, PICKUP_OFFSET);
		}else{
			circle.center = new Vector2(PICKUP_OFFSET, DEFAULT_Y_OFFSET);
		}
	}
	
	void OnTriggerEnter2D(Collider2D other){
		//keep a list of items we can grab
		Holdable otherHoldable;
		try{
			otherHoldable = (Holdable) other.GetComponent (typeof(Holdable));
		}catch(InvalidCastException){
			return; //not a holdable
		}
		
		if(otherHoldable == player.rightHandItem || otherHoldable == player.leftHandItem){
			return; //already holding this
		}
		
		player.holdables.Add (otherHoldable);
		
	}
	
	void OnTriggerExit2D(Collider2D other){
		//remove items from list when they're no longer grabbable
		Holdable otherHoldable;
		try{
			otherHoldable = (Holdable) other.GetComponent (typeof(Holdable));
		}catch(InvalidCastException){
			return; //not a holdable
		}
		
		player.holdables.Remove (otherHoldable);
	}
}
