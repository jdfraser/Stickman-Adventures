using UnityEngine;
using System;
using System.Collections;

public class ZoneBorder : MonoBehaviour {
	public GameObject respawnPoint;
	public float respawnTime = 1.5f;

	void OnTriggerEnter2D(Collider2D other){
		if(other.transform.root.name == "player"){
			//this is the player. Let them know they're dead
			PlayerController player = (PlayerController) other.GetComponent<PlayerController>();
			if(player != null){
				player.die(respawnTime);
			}

		}else{
			//not the player. destroy the object
			if(! other.CompareTag ("dnd")){
				//not the player, not a do-not-destroy, so kill it
				Destroy (other.gameObject);
			}
		}

	}
}
