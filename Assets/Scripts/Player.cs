using UnityEngine;
using System.Collections;
using CnControls;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {
    float moveSpeed = 6;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    /**
        These 2 values maxJumpHeight and timeToJumpApex are meant 
        to be set directly, gravity and 
        maxJumpVelocity are set using these values via equations
    **/
    float maxJumpHeight = 6;
    float minJumpHeight = 1;
    float timeToJumpApex = .4f;

    //public Vector2 wallJumpClimb;   //climb up
    //public Vector2 wallJumpOff;      //pop off wall
    public Vector2 wallLeap;        // jump off the wall
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    float velocityXSmoothing;

    Vector3 velocity;

    Controller2D controller;

    void Start() {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void Update() {
        Vector2 input = new Vector2(CnInputManager.GetAxisRaw("Horizontal"), CnInputManager.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(
            velocity.x,
            targetVelocityX,
            ref velocityXSmoothing,
            (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne)
            );

        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
            wallSliding = true; // Set sprites here if wall jumping

            if(velocity.y < -wallSlideSpeedMax) {
                velocity.y = -wallSlideSpeedMax;
            }

            if(timeToWallUnstick > 0) {
                velocity.x = 0;
                velocityXSmoothing = 0;
                if(input.x != wallDirX && input.x != 0) {
                    timeToWallUnstick -= Time.deltaTime;
                } else {
                    timeToWallUnstick = wallStickTime;
                }
            } else {
                timeToWallUnstick = wallStickTime;
            }
        }

        //Jump portion, may not be using in actual 2.5d implementation
        if(CnInputManager.GetButtonDown("Jump")) {
            if (wallSliding) {
                /*if(wallDirX == input.x) {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                } else if(input.x == 0) {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                } else {*/
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
                
            }
            if (controller.collisions.below) {
                velocity.y = maxJumpVelocity;
            }
        }
        if (CnInputManager.GetButtonUp("Jump")) {
            if(velocity.y > minJumpVelocity) {
                velocity.y = minJumpVelocity;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, input);

        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
    }
}
