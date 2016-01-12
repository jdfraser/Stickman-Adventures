using UnityEngine;
using System.Collections;

public class Spear : ComplexObject {
	private PlayerController player;

	public float throwSpeed;
	private const float THROW_TIME = 0.65f;

	void Start () {
		player = (PlayerController) FindObjectOfType (typeof(PlayerController));
	}

	public override void use(){
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -transform.localEulerAngles.z); //flip the spear
		player.throwSpear();
		Invoke ("getThrown", THROW_TIME);
	}

	public override void pickup(Transform newParent){
		Vector2 scale = transform.localScale;

		base.pickup (newParent);

		transform.localScale = scale;
		
		Destroy(this.rigidbody2D);
		collider2D.enabled = false;
		transform.GetChild (0).collider2D.enabled = false;

		float offset = (collider2D.bounds.size.x / transform.localScale.x * 0.5f); //offset by half the length of the line, accounting for scaling
		
		transform.Rotate (0, 0, -90);

		if(transform.parent.name == "hand_right"){
			transform.localPosition = new Vector3 (0, -offset, 0);
		}else{
			transform.localPosition = new Vector3 (0, offset, 0);
		}
	}

	public override void drop(){
		Vector2 scale = transform.localScale;
		Transform root = transform.root;
		
		base.drop ();
		
		transform.localScale = scale;

		gameObject.AddComponent<Rigidbody2D>();
		collider2D.enabled = true;
		transform.GetChild (0).collider2D.enabled = true;
		
		if(root.localScale.x < 0){
			//facing right
			transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -transform.eulerAngles.z);
		}
	}

	private void getThrown(){
		Vector2 scale = transform.localScale;
		Transform root = transform.root;
		
		base.drop ();
		
		transform.localScale = scale;

		
		gameObject.AddComponent<Rigidbody2D>();
		collider2D.enabled = true;
		transform.GetChild (0).collider2D.enabled = true;

		if(root.localScale.x > 0){
			//facing right
			rigidbody2D.velocity = new Vector2(throwSpeed, 0);
			transform.localEulerAngles = Vector3.zero;
		}else{
			//facing left
			rigidbody2D.velocity = new Vector2(-throwSpeed, 0);
			transform.localEulerAngles = new Vector3(0, 0, 180);
		}
	}
}
