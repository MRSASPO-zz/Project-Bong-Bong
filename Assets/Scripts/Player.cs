using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {
    float moveSpeed = 6;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    /**
        These 2 values jumpHeight and timeToJumpApex are meant 
        to be set directly, gravity and 
        jumpVelocity are set using these values via equations
    **/
    float jumpHeight = 4;
    float timeToJumpApex = .4f;

    float gravity;
    float jumpVelocity;
    float velocityXSmoothing;

    Vector3 velocity;

    Controller2D controller;

    void Start() {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void Update() {

        if(controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
        //Jump portion, may not be using in actual 2.5d implementation
        if(Input.GetKeyDown(KeyCode.Space) && controller.collisions.below) {
            velocity.y = jumpVelocity;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        float targetVelocityX = input.x * moveSpeed;

        velocity.x = Mathf.SmoothDamp(
            velocity.x, 
            targetVelocityX, 
            ref velocityXSmoothing, 
            (controller.collisions.below? accelerationTimeGrounded: accelerationTimeAirborne)
            );

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
