using UnityEngine;
using System.Collections;

public class Action_ResetLevel : Action {

	public override void execute(){
		GameLogic.instance.setCameraStart (Camera.main.transform.position);
		Application.LoadLevel (Application.loadedLevel);
	}

	private void resetLevel(){

	}
}
