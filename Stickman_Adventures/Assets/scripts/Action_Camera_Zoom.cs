using UnityEngine;
using System.Collections;

public class Action_Camera_Zoom : Action {
	public float size = 15f;
	public float increment = 1; //set to minus to decrement
	public bool zooming = false;

	// Use this for initialization
	void Start () {
		Camera.main.orthographicSize = 15f;
	}
	
	// Update is called once per frame
	void Update () {
		if(zooming && increment > 0){
			//growing
			if(Camera.main.orthographicSize < size){
				Camera.main.orthographicSize += increment * (1/size);
			}else{
				zooming = false;
			}
		}else if(zooming && increment < 0){
			//shrinking
			if(Camera.main.orthographicSize > size){
				Camera.main.orthographicSize += increment * (1/size);
			}else{
				zooming = false;
			}
		}

	}

	public override void execute(){
		zooming = true;
	}
}
