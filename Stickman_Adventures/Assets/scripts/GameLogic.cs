using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {
	
	public static GameLogic instance { get; private set; }
	public Hashtable combinations;

	private static Vector3 cameraPos;
	private static bool defaultCamera = true;

	void Awake(){
		instance = this;
	}
	
	// Use this for initialization
	void Start () {
		combinations = new Hashtable();
		combinations.Add ("circle", new Hashtable() );
		Hashtable temp = (Hashtable)combinations ["circle"];
		temp.Add ("line", "balloon");

		combinations.Add ("line", new Hashtable ());
		temp = (Hashtable)combinations ["line"];
		temp.Add ("circle", "balloon");
		temp.Add ("triangle", "spear");

		combinations.Add ("triangle", new Hashtable() );
		temp = (Hashtable)combinations ["triangle"];
		temp.Add ("line", "spear");

	}

	public void setCameraStart(Vector3 pos){
		defaultCamera = false;
		cameraPos = pos;
	}

	public Vector3 getCameraStart(){
		//gives the non-default camera start position, then resets to default
		//this is so we don't interfere with default positions in other levels when we load them
		defaultCamera = true;
		return cameraPos;
	}

	public bool isDefaultCamera(){
		return defaultCamera;
	}
}
