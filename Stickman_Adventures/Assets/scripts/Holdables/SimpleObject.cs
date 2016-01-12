using UnityEngine;
using System.Collections;
using System;

public class SimpleObject : Holdable {

	public Transform poof; //the particle system used for the clouds on item combine

	void Awake(){
		parent = null;
	}

	void Start () {

	}
	
	void Update () {
		
	}
	
	public override void drop(){
		Vector3 worldpos = transform.position;
		
		transform.parent.DetachChildren ();
		
		transform.localScale = Vector3.one;
		transform.localPosition = worldpos;

		if(parent.root.localScale.x < 0){ 
			//flip if player was flipped when we were dropped to match player's direction
			transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -transform.eulerAngles.z);
		}

		gameObject.AddComponent<Rigidbody2D>();
		collider2D.enabled = true;
	}
	
	public override void pickup(Transform newParent){
		parent = newParent;
		
		gameObject.layer = 9; // set to a layer that does not collide with player.

		transform.parent = parent;
		
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;

		Destroy(this.rigidbody2D);
		collider2D.enabled = false;
		collider2D.isTrigger = false;
	}
	
	public GameObject combine(Holdable other){
		Hashtable combos = (Hashtable) GameLogic.instance.combinations [shape];
		string newShapeName = (string) combos [other.shape];
		GameObject newShape;
		try{
			newShape = (GameObject) Instantiate (Resources.Load ("prefabs/" + newShapeName));
		}catch(ArgumentException){
			return null;
		}

		Transform particles = (Transform) Instantiate (poof, transform.position, Quaternion.identity);
		particles.renderer.sortingLayerName = "Foreground";

		return newShape;
	}

	public override bool isHeld(){
		if(parent != null){
			return true;
		}

		return false;
	}

	private void setPivotEdge(){
		Sprite newSprite = Resources.Load<Sprite> ("sprites/" + shape + "s/" + shape + "_pivot_edge");
		if(newSprite == null){
			//Debug.Log ("Could not load resource " + shape + "_pivot_edge");
			return; //couldn't find that sprite, just ignore
		}
		SpriteRenderer spriterenderer = GetComponent<SpriteRenderer>();
		spriterenderer.sprite = newSprite;
		
	}
	
	private void setPivotCentre(){
		Sprite newSprite = Resources.Load<Sprite> ("sprites/" + shape + "s/" + shape + "_pivot_centre");
		if(newSprite == null){
			//Debug.Log ("Could not load resource " + shape + "_pivot_centre");
			return; //couldn't find that sprite, just ignore
		}
		SpriteRenderer spriterenderer = GetComponent<SpriteRenderer>();
		spriterenderer.sprite = newSprite;
		
	}
}
