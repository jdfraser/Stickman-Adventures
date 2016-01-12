using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if(other.transform.root.name == "player"){
			PlayerController player = other.GetComponent<PlayerController>();
			player.waterEnter();
		}else if(other.transform.root.name == "halfcircle"){
			HalfCircle hc = other.GetComponent<HalfCircle>();
			hc.waterEnter();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.transform.root.name == "player"){
			PlayerController player = other.GetComponent<PlayerController>();
			player.waterExit();
		}else if(other.transform.root.name == "halfcircle"){
			HalfCircle hc = other.GetComponent<HalfCircle>();
			hc.waterExit();
		}
	}
}
