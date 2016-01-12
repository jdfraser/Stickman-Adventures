using UnityEngine;
using System;
using System.Collections;

public class ShootProjectile : MonoBehaviour {
	private DateTime prevTime;
	private DateTime currTime;

	public int interval; //in milliseconds
	public Transform projectile;
	public Transform origin;
	public float projectileSpeed;
	public float projectileMass;
	public string materialName = "bounce_hard";
	public AudioSource boom;

	void Awake(){

	}
	// Use this for initialization
	void Start () {
		prevTime = System.DateTime.Now;
	}
	
	// Update is called once per frame
	void Update () {
		currTime = System.DateTime.Now;
		TimeSpan diff = currTime - prevTime;

		if(diff.TotalSeconds > interval){
			//enough time has passed. Spawn the target object.
			boom.Play ();
			GameObject temp = (GameObject) Instantiate (projectile, origin.transform.position, Quaternion.identity);
			temp.rigidbody2D.velocity = origin.transform.forward * projectileSpeed;
			temp.rigidbody2D.mass = projectileMass;
			temp.collider2D.sharedMaterial = (PhysicsMaterial2D) Resources.Load ("materials/" + materialName);
			temp.collider2D.enabled = false; //workaround for unity bug where material is assigned but doesn't take effect
			temp.collider2D.enabled = true;
			prevTime = currTime;
		}
	}
}
