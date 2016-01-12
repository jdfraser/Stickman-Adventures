using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public GameObject target;
	public float speed;

	private bool frozen = false;

	void Start(){
		if(! GameLogic.instance.isDefaultCamera()){
			transform.position = GameLogic.instance.getCameraStart ();
		}
	}

	// Update is called once per frame
	void Update () {
		if(frozen){
			return;
		}
		float x = transform.position.x + (target.transform.position.x - transform.position.x) * speed;
		float y = transform.position.y + (target.transform.position.y - transform.position.y) * speed;
		float z = transform.position.z; //use own z--don't change depth

		transform.position = new Vector3 (x, y, z);
	}

	public void freeze(){
		frozen = true;
	}

	public void unfreeze(){
		frozen = false;
	}
}
