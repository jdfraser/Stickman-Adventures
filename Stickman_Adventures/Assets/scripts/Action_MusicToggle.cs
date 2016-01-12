using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Action_MusicToggle : Action {
	Music music;

	void Awake(){
		try{
			music = GameObject.Find ("music").GetComponent<Music>();
		}catch(NullReferenceException){
			music = null; //mostly for dev purposes so we don't constantly get errors about missing music
		}
	}

	public override void execute(){
		if(music == null){ //in case music object isn't in this scene for some reason
			return;
		}
		music.toggle();

		Text label = GetComponentInParent<Text>();
		if(music.isOn ()){
			label.text = "MUSIC ON";
		}else{
			label.text = "MUSIC OFF";
		}
	}
}
