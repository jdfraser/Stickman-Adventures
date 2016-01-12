using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {
	void Start () {
		Application.LoadLevel ("start_menu");	
		Screen.showCursor = false;
		Application.runInBackground = true;
	}
}
