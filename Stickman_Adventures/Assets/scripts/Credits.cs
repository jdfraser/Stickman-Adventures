using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	
	void Update () {
		if(Input.anyKey){
			Application.LoadLevel("start_menu");
		}
	}
}
