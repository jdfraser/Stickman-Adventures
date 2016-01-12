using UnityEngine;
using System;
using System.Collections;

public class PlayerController : MonoBehaviour {
	/*============CONSTANTS============*/
	private const float ONGROUND_OFFSET = 0.5f; //how wide the "cone" for the on ground check is
	private const float WALL_PADDING = 0.2f;
	private const float DIST_TO_GROUND = 5.9f; //dist from player position to ground
	private const float THROW_TIME = 0.83f; //change this to whatever exit time of spear throw is

	/*============SETTINGS============*/
	public float walkSpeed;
	public float runSpeed;
	public float jumpStrength;
	public Animator anim;
	public GameObject spawnPoint;

	/*============SOUNDS============*/
	public AudioSource sound_footstep;
	public AudioSource sound_whoosh;
	public AudioSource sound_poof;
	public AudioSource sound_thud;
	public AudioSource sound_pickup;
	public AudioSource sound_error;

	//used for checking what inputs were given this frame
	//kept as separate bools to decouple checks from behaviour

	/*============STATE============*/
	private bool collidingTop;
	private bool collidingLeft;
	private bool collidingRight;
	private bool onGround;
	private bool frozen;
	private bool dead;
	private bool inWater;

	private bool jumped;
	private bool walked;
	private bool ran;
	private bool balloon;
	private bool grabbed;
	private bool dropped;
	private bool combined;
	private bool throwRight;
	private bool throwLeft;
	private bool used;
	private bool menuOpen;
	private bool lookedDown;
	private bool lookedUp;

	/*============INTERNAL OBJECTS============*/
	private Transform rightHand;
	private Transform leftHand;
	private Menu menu;

	public Holdable rightHandItem = null;
	public Holdable leftHandItem = null;
	public ArrayList holdables = new ArrayList();



	/*=================SETUP===================*/

	void Awake(){
		rightHand = GameObject.Find ("hand_right").transform;
		leftHand = GameObject.Find ("hand_left").transform;

		if(spawnPoint != null){
			transform.position = spawnPoint.transform.position;
		}
	}

	void Start(){
		menu = GameObject.Find ("menu").GetComponent<Menu>();
		if(rightHandItem != null){
			rightHandItem.pickup (rightHand);
		}
		if(leftHandItem != null){
			leftHandItem.pickup (leftHand);
		}
	}


	/*=================MAIN LOOP===================*/

	void Update(){
		handleState ();
		handleItems ();
		handlePhysics();
		handleSound ();
		setAnims ();
	}

	/*=================STATE===================*/

	private void handleState(){
		if(throwRight || throwLeft){
			return;
		}

		if(Input.GetKeyUp (KeyCode.Escape)){
			menuOpen = !menuOpen;
			if(menuOpen){
				menu.show ();
			}else{
				menu.hide ();
			}
		}

		if(frozen || menuOpen){
			return;
		}

		if(Input.GetKeyDown(KeyCode.Space) && onGround){
			jumped = true;
		}else{
			jumped = false;
		}

		if(Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.D)){
			//moving, not holding shift, therefore walking
			walked = true;

			if(onGround){
				if(Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)){
					//moving and holding shift--running
					ran = true;
				}else{
					ran = false;
				}
			}

		}else{
			//not moving at all
			walked = false;
			ran = false;
		}

		if(Input.GetKeyUp(KeyCode.E)){
			grabbed = true;
		}else{
			grabbed = false;
		}
		
		if(Input.GetKeyUp (KeyCode.Q)){
			dropped = true;
		}else{
			dropped = false;
		}
		
		if(Input.GetKeyUp (KeyCode.F)){
			combined = true;
		}else{
			combined = false;
		}

		if(Input.GetKeyUp (KeyCode.R)){
			used = true;
		}else{
			used = false;
		}

		if(Input.GetKey (KeyCode.S)){ //for look up/down animations. NOTE: handling of pickuprange placement is done in that script
			lookedDown = true;
		}else{
			lookedDown = false;
		}

		if(Input.GetKey (KeyCode.W)){
			lookedUp = true;
		}else{
			lookedUp = false;
		}

	}



	/*=================PHYSICS===================*/
	
	private void handlePhysics(){
		//check if we're on the ground
		onGround = checkCollision (transform.position, -Vector2.up, DIST_TO_GROUND);
		if(! onGround){
			//nothing directly below us--let's check left and right
			Vector2 right = new Vector2 (ONGROUND_OFFSET, -DIST_TO_GROUND).normalized; // down and right
			Vector2 left = new Vector2 (-ONGROUND_OFFSET, -DIST_TO_GROUND).normalized; // down and left
			if(checkCollision (transform.position, right, DIST_TO_GROUND) || checkCollision(transform.position, left, DIST_TO_GROUND)){
				onGround = true;
			}
		}
		
		collidingTop = checkCollision (transform.position, Vector2.up, 1.58f);

		if(anim.GetCurrentAnimatorStateInfo(0).IsName ("ThrowRight") || anim.GetCurrentAnimatorStateInfo(0).IsName("ThrowLeft")){
			return;
		}

		if(frozen || menuOpen){
			return;
		}
	
		float h = Input.GetAxis ("Horizontal");
		float v = rigidbody2D.velocity.y; //current vertical velocity

		if(h < 0){
			//moving left
			transform.localScale = new Vector3 (-1, 1);
		}else if(h > 0){
			//moving right
			transform.localScale = new Vector3 (1, 1);
		}

		//if we're in the air and hit a wall, don't allow movement in that direction
		//prevents player from "catching" on walls/platforms
		if(h > 0 && collidingRight){
			h = 0;
		}else if(h < 0 && collidingLeft){
			h = 0;
		}

		if(onGround && jumped){
			v = jumpStrength;
		}

		if(ran && ! balloon){
			h *= runSpeed;
		}else if(walked){
			h *= walkSpeed;
		}

		rigidbody2D.velocity = new Vector2 (h, v);
	}

	private bool checkCollision(Vector2 origin, Vector2 direction, float length){
		RaycastHit2D[] hits = Physics2D.RaycastAll (origin, direction, length);
		foreach(RaycastHit2D hit in hits){
			if(hit.transform.GetComponent<Collider2D>() == null){
				continue;
			}

			if(hit.transform.GetComponent<Collider2D>().isTrigger){
				continue;
			}

			if(hit.transform.name != transform.name){
				return true;
			}
		}

		return false;
	}

	void OnCollisionStay2D(Collision2D other){
		//check if colliding horizontally and NOT vertically
		//used in handlePhysics to handle jumping/falling against walls.
		collidingLeft = false;
		collidingRight = false;
		if(! onGround && ! collidingTop){
			//top and bottom raycasts failed. We're colliding on our side
			if(other.transform.position.x > transform.position.x){
				collidingRight = true;
			}else{
				collidingLeft = true;
			}
		}
	}

	void OnCollisionExit2D(Collision2D other){
		collidingLeft = false; //TODO: currently have to set these to false both on collision stay and collision exit.
		collidingRight = false; //can we combine them somewhere? seems to be a problem doing it in handlePhysics()
	}


	/*=================ANIMATION===================*/

	private void setAnims(){
		anim.SetBool ("Jumping", jumped);
		anim.SetBool ("Walking", walked);
		anim.SetBool ("Running", ran);
		anim.SetBool ("OnGround", onGround);
		anim.SetBool ("Balloon", balloon);
		anim.SetBool ("ThrowRight", throwRight);
		anim.SetBool ("ThrowLeft", throwLeft);
		anim.SetBool ("LookDown", lookedDown);
		anim.SetBool ("LookUp", lookedUp);
	}

	/*=================ITEMS===================*/

	//items are added to/removed from holdables list by the pickup_range object's script
	//the holdables list is an up-to-date list of all current objects that can be picked up

	void handleItems(){
		if(grabbed){
			if(rightHandItem == null){
				grab (rightHand, ref rightHandItem);
			}else if(leftHandItem == null){
				grab(leftHand, ref leftHandItem);
			}
		}

		if(dropped){
			if(leftHandItem != null){
				drop(ref leftHandItem);
			}else if(rightHandItem != null){
				drop(ref rightHandItem);
			}
		}
		
		if(combined){
			combine();
		}

		if(used){
			if(rightHandItem is ComplexObject){
				((ComplexObject) rightHandItem).use ();
			}else if(leftHandItem is ComplexObject){
				((ComplexObject) leftHandItem).use ();
			}
		}
		
		if(rightHandItem != null && rightHandItem.shape == "balloon"){
			//we're holding a balloon
			balloon = true;
			Balloon b = rightHandItem.GetComponent<Balloon>();
			if(b.popped){
				rightHandItem.drop ();
				rightHandItem = null;
			}
		}else if(leftHandItem != null && leftHandItem.shape == "balloon"){
			//we're holding a balloon
			balloon = true;
			Balloon b = leftHandItem.GetComponent<Balloon>();
			if(b.popped){
				leftHandItem.drop ();
				leftHandItem = null;
			}
		}else{
			//no balloon
			balloon = false;
		}
		
	}

	private void grab(Transform hand, ref Holdable currentItem){
		Holdable item;

		item = closestHoldable();

		if(item == null){
			//nothing to pick up
			return;
		}

		if(item.CompareTag("dnp")){
			//marked as do not pick up
			return;
		}

		//passed checks, let's pick something up
		item.pickup (hand);
		currentItem = item;
		playSound_pickup ();
	}

	private void drop(ref Holdable currentItem){
		currentItem.drop();
		holdables.Remove (currentItem);
		currentItem = null;
	}

	private void combine(){
		if(rightHandItem == null || leftHandItem == null){ //not holding something in both hands. we're done.
			return;
		}

		SimpleObject rightSimple;
		SimpleObject leftSimple;

		try{
			rightSimple = (SimpleObject) rightHandItem;
			leftSimple = (SimpleObject) leftHandItem;
		}catch(InvalidCastException){
			//one of these things is not a simple object, so not combineable. we're done.
			playSound_error ();
			return;
		}

		GameObject newObject = rightSimple.combine (leftSimple);

		if(newObject == null){
			playSound_error ();
			return;
		}

		Destroy (leftHandItem.gameObject);
		Destroy (rightHandItem.gameObject);
		
		ComplexObject newItem = (ComplexObject) newObject.GetComponent (typeof(ComplexObject));

		newItem.pickup (rightHand);
		rightHandItem = newItem;

		playSound_poof ();
	}

	private Holdable closestHoldable(){
		float dist;
		float oldDist = float.MaxValue;
		Holdable closest = null;

		foreach(Holdable holdable in holdables){
			if(holdable == rightHandItem || holdable == leftHandItem){
				//we're already holding this, ignore it
				continue;
			}

			dist = Vector2.Distance(transform.position, holdable.transform.position);

			if(dist < oldDist){
				closest = holdable;
				oldDist = dist;
			}
		}

		return closest;
	}

	public void throwSpear(){
		if(rightHandItem.shape == "spear"){
			throwRight = true;
			rightHandItem = null;
		}else if(leftHandItem.shape == "spear"){
			throwLeft = true;
			leftHandItem = null;
		}
		Invoke ("throwDone", THROW_TIME);
	}

	void throwDone(){
		//EXPLANATION: originally polled animator for current animation, and switched off flags when current anim was not throwright/throwleft.
		//if on, locked out all input
		//problem: too much delay on call to animator. Input still received for some time after start of throw, and flags switched off prematurely
		//(before animation had started)
		//instead, now just scheduling flags to switch off at exit time of throw animation
		throwLeft = false;
		throwRight = false;
	}




	/*=================SOUND===================*/

	private void handleSound(){

	}

	public void playSound_walk(){
		if(onGround){
			sound_footstep.pitch = 1.3f;
			sound_footstep.volume = 0.25f;
			sound_footstep.Play ();
		}
	}

	public void playSound_run(){
		if(onGround){
			sound_footstep.pitch = 3f;
			sound_footstep.volume = 0.3f;
			sound_footstep.Play();
		}
	}

	public void playSound_whoosh(){
		sound_whoosh.Play ();
	}

	public void playSound_poof(){
		sound_poof.Play ();
	}

	public void playSound_thud(){
		sound_thud.Play ();
	}

	public void playSound_pickup(){
		sound_pickup.Play ();
	}

	public void playSound_error(){
		sound_error.Play ();
	}




	/*=================MISC===================*/
	
	public void reset(){
		jumped = false;
		ran = false;
		combined = false;
		
		if(rightHandItem != null){
			Destroy (rightHandItem.gameObject);
			rightHandItem = null;
		}
		
		if(leftHandItem != null){
			Destroy(leftHandItem.gameObject);
			leftHandItem = null;
		}
		
		holdables.Clear ();
		
	}

	public void die(float respawnTime){
		if(dead){
			return;
		}
		GameObject poof = (GameObject) Resources.Load ("prefabs/death_poof");
		CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
		camera.freeze ();
		GameObject particles = (GameObject) Instantiate (poof, transform.position, Quaternion.identity);
		particles.renderer.sortingLayerName = "Foreground";
		
		Invoke ("resetLevel", respawnTime);
		playSound_thud ();
		dead = true;

	}

	public void die_explode(float respawnTime){
		if(dead){
			return;
		}
		anim.Play ("Explode"); //switch to animation instantly to avoid known issue where animator delays a couple seconds before switching
		frozen = true;
		rigidbody2D.isKinematic = true;
		Invoke ("resetLevel", respawnTime);
		playSound_thud ();
		dead = true;
	}

	public void resetLevel(){
		GameLogic.instance.setCameraStart (Camera.main.transform.position);
		Application.LoadLevel (Application.loadedLevel);
	}

	public void freeze(){
		frozen = true;
	}

	public void silent(){
		//used by door to prevent us from hearing player movement during level transitions
		AudioSource[] sounds = GetComponents<AudioSource>();
		foreach(AudioSource sound in sounds){
			sound.enabled = false;
		}
	}

	public void waterEnter(){
		inWater = true;
	}

	public void waterExit(){
		inWater = false;
	}
}
