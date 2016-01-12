using UnityEngine;
using System.Collections;
using System;

public class door : MonoBehaviour {
	public string targetLevel;
	public GameObject wipeTarget;

	void OnTriggerEnter2D(Collider2D other){
		PlayerController player = other.GetComponent<PlayerController>();
		if(player != null && other.GetType() == typeof(PolygonCollider2D)){
			//this is the player (and they collided with their body, not their grabbing radius). Send them to the next level
			Invoke ("nextLevel", 2f);
			CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
			cam.target = wipeTarget;
			player.freeze();
			player.silent ();
		}
	}

	void nextLevel(){
		Application.LoadLevel (targetLevel);
	}
}
