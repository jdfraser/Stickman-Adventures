using UnityEngine;
using System.Collections;

public class ZoneSpike : MonoBehaviour {
	public GameObject respawnPoint;
	public float respawnTime = 1.5f;
	
	void OnTriggerEnter2D(Collider2D other){
		if(other.transform.root.name == "player"){
			//this is the player. Let them know they're dead
			PlayerController player = (PlayerController) other.GetComponent<PlayerController>();
			if(player != null){
				player.die_explode(respawnTime);
			}
			
		}
		
	}
}
