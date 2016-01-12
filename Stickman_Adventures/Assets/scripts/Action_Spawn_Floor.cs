using UnityEngine;
using System.Collections;

public class Action_Spawn_Floor : Action {

	public override void execute(){
		Animator anim = GetComponent<Animator> ();
		anim.SetBool ("Spawn1", true);
	}
}
