using UnityEngine;
using System.Collections;

public class MainMenu : Menu {

	protected override void select(int selection){
		base.select (selection);
		sound_select.Play ();
		hide ();
	}
}
