using UnityEngine;
using System.Collections;

public class Action_Quit : Action {

	public override void execute(){
		Application.LoadLevel ("start_menu");
	}
}
