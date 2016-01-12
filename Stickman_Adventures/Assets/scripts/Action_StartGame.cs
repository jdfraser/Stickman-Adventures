using UnityEngine;
using System.Collections;

public class Action_StartGame : Action {
	public override void execute(){
		Invoke ("moveCamera", 0.5f);
	}

	private void moveCamera(){
		Camera c = Camera.main;
		CameraFollow camera = c.GetComponent<CameraFollow>();
		camera.target = GameObject.Find ("target2");
		Invoke ("startGame", 1f);
	}
	
	private void startGame(){
		Application.LoadLevel ("level_1");
	}
}
