using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	public Transform target;
	public AudioSource sound_beep;

	private bool pressed = false;

	void OnTriggerEnter2D(Collider2D other){
		if(other.transform.root.name == "player"){
			return;
		}
		press ();
	}

	private void press(){
		Action buttonAction = target.GetComponent<Action> ();
		pressed = true;
		Animator anim = transform.parent.GetComponent<Animator> ();
		anim.SetBool ("Pressed", pressed);
		buttonAction.execute ();
		sound_beep.Play ();
	}

}
