using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {
	private Menu menu;
	private GameObject pressEnter;

	void Awake(){
		menu = GameObject.Find ("menu").GetComponent<Menu>();
		pressEnter = GameObject.Find ("press_enter");
	}

	void Update () {
		if(Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp (KeyCode.KeypadEnter)){
			menu.show ();
			pressEnter.SetActive(false);
			gameObject.SetActive(false);
		}
	}
}
