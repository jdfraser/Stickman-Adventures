using UnityEngine;
using System.Collections;

public class HalfCircle : SimpleObject {
	private bool inWater = false;
	public float buoyancy = 30f;
	public float waterDrag = 2f;
	public float waterADrag = 50f;
    public float waterMass = 20f;
	public float waterGrav = 0f;
    public float bobDist = 0.2f;
    
    private Transform surface;
	private float landDrag = 0f;
	private float landADrag = 0.05f;
	private float landMass = 1f;
	private float landGrav = 1f;
    private float distToSurface;
    private bool goingDown = false;
    
	void Start(){
        surface = GameObject.Find("water_surface").transform;
	}

	void Update(){
        if (rigidbody2D == null) { //currently attached to something
            return;
        }
        distToSurface = Mathf.Abs(surface.position.y - transform.position.y);
        //Debug.Log(distToSurface);
		if(inWater){
            if(transform.position.y < surface.position.y && distToSurface > bobDist) {
                goingDown = false; //reached maximum bob depth, should start moving back up
            } else if (transform.position.y > surface.position.y && distToSurface > bobDist) {
                goingDown = true; //just came up above the surface, time to head back down
            }
            Vector2 force;
            if (goingDown) {
                force = -Vector2.up * (buoyancy - (distToSurface * 12f));
            } else {
                force = Vector2.up * (buoyancy + (distToSurface * 12f));
            }
			rigidbody2D.AddForce(force, ForceMode2D.Force);

            if (Mathf.Abs(transform.eulerAngles.z - 180) > 3) { //more than 3 degrees off from straight up
                if (transform.eulerAngles.z > 180) { //TODO: possibly lerp this rotation? 
                    transform.Rotate(0, 0, -1);
                } else {
                    transform.Rotate(0, 0, 1);
                }
            }
        }
	}

    public override void pickup(Transform newParent) {
        float offset = -(collider2D.bounds.size.x / transform.localScale.x * 0.5f); //offset by half the width of the circle, accounting for scaling
        base.pickup(newParent);

        transform.localPosition = new Vector3(offset, 0, 0);
    }
	
	public void waterEnter(){
        if (rigidbody2D == null) {
            return;
        }
		rigidbody2D.drag = waterDrag;
		rigidbody2D.angularDrag = waterADrag;
        rigidbody2D.gravityScale = waterGrav;
        rigidbody2D.mass = waterMass;
		inWater = true;
	}
	
	public void waterExit(){
        if (rigidbody2D == null) {
            return;
        }
		rigidbody2D.drag = landDrag;
		rigidbody2D.angularDrag = landADrag;
        rigidbody2D.gravityScale = landGrav;
        rigidbody2D.mass = landMass;
		inWater = false;
	}

}
