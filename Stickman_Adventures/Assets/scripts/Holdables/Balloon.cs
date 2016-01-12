using UnityEngine;
using System.Collections;

public class Balloon : ComplexObject {
	const float PICKUP_OFFSET = -2f;

	public float stringLength = 8f;
	public float newPlayerGravity = -0.5f;
	public float newLinearDrag = 7f;
	public AudioSource sound_pop;

	private LineRenderer balloonString;
	private DistanceJoint2D joint;
	public bool popped = false;
	
	void Awake(){
		balloonString = GetComponentInChildren<LineRenderer>();
		joint = GetComponent<DistanceJoint2D> ();
		joint.distance = stringLength;
	}
	
	void Update () {
		if(isHeld ()){ //attach the string to the player's hand
			if(popped && Vector2.Distance (transform.position, transform.parent.position) < 1){
				//we're close to our anchor and popped. Time to die.
				Destroy (this.gameObject);
     		}
			balloonString.SetPosition (0, transform.position);
			balloonString.SetPosition (1, transform.parent.position);
		}else{
			balloonString.SetPosition(0, Vector2.zero);
			balloonString.SetPosition(1, new Vector2(stringLength, 0));
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.transform.name.Contains("spear") || other.transform.name.Contains("spike")){
			if(! popped){
				pop ();
			}
		}
	}

	public override void pickup(Transform newParent){
		transform.parent = newParent;

		gameObject.layer = 9;

		joint.connectedBody = newParent.rigidbody2D;
		joint.connectedAnchor = Vector2.zero;
		joint.enabled = true;

		transform.localPosition = Vector2.zero;
		transform.localScale = Vector2.one;
		transform.localRotation = Quaternion.identity;
		transform.Rotate (0, 0, -30); //no idea why this needs to happen. Works though. Look into this if problem occurs with other items.

		transform.parent.root.rigidbody2D.gravityScale = newPlayerGravity;
		transform.parent.root.rigidbody2D.drag = newLinearDrag;

		balloonString.useWorldSpace = true;
	}

	public override void drop(){
		transform.parent.root.rigidbody2D.gravityScale = 8f;
		transform.parent.root.rigidbody2D.drag = 0f;
		transform.parent.DetachChildren ();

		transform.localRotation = Quaternion.identity;
		transform.Rotate (0, 0, 270); //so string will point straight down

		joint.enabled = false;

		balloonString.useWorldSpace = false;
	}

	private void pop(){
		GameObject poof = (GameObject) Resources.Load ("prefabs/death_poof");
		GameObject particles = (GameObject) Instantiate (poof, transform.position, Quaternion.identity);
		particles.renderer.sortingLayerName = "Foreground";
		SpriteRenderer rend = GetComponent<SpriteRenderer> ();
		collider2D.enabled = false;
		Destroy (rend); //makes us invisible
		rigidbody2D.gravityScale = 10;
		joint.maxDistanceOnly = true; //so the string will drop
		popped = true;

		sound_pop.Play ();
	}
}
