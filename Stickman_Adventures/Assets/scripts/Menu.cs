using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public AudioSource sound_arrow;
	public AudioSource sound_select;
	private Animator anim;
	private int currentSelection = 0;
	private bool showing = false;

	public GameObject[] arrows;
	
	void Awake(){
		anim = GetComponent<Animator> ();
	}

	void Update(){
		if(showing){
			for(int i = 0; i < arrows.Length; i++){
				if(i == currentSelection){
					arrows[i].SetActive(true);
				}else{
					arrows[i].SetActive(false);
				}
			}

			if (Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.DownArrow)) {
				selectDown();
			}
			if(Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.UpArrow)){
				selectUp();
			}
			if(Input.GetKeyUp (KeyCode.Return) || Input.GetKeyUp (KeyCode.KeypadEnter)){
				select(currentSelection);
			}
		}
	}

	public void show(){
		showing = true;
		currentSelection = 0;
		anim.SetBool ("Show", true);
	}

	public void hide(){
		showing = false;
		anim.SetBool ("Show", false);
	}

	private void selectDown(){
		sound_arrow.Play ();
		currentSelection ++;
		if(currentSelection > arrows.Length - 1){
			currentSelection = 0;
		}
	}

	private void selectUp(){
		sound_arrow.Play ();
		currentSelection --;
		if(currentSelection < 0){
			currentSelection = arrows.Length - 1;
		}
	}

	protected virtual void select(int selection){
		Action a = (Action) arrows [selection].GetComponent<Action> ();
		a.execute ();

	}
}
