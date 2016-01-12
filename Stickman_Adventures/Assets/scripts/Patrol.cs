using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour {
    public float moveSpeed; //set to negative to move left, positive to move right


    void Update(){
        rigidbody2D.velocity = new Vector2(moveSpeed, 0);
        transform.rotation = Quaternion.identity;
    }

    void OnCollisionEnter2D(Collision2D other) {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        moveSpeed *= -1;
    }
}
