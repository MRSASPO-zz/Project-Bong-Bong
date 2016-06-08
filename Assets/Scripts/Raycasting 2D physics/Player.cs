using UnityEngine;
using System.Collections;
using CnControls;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {
    /**
        These values are public and can be changed at any moment if 
        the default values arent desired through the editor
    **/
    //public Values that deal with character movement are in this section
    public float moveSpeed = 10;
    public float maxJumpHeight = 6;
    public float minJumpHeight = 1;
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    public float timeToWallUnstick = 0;
    public Vector2 wallLeap = new Vector2(10, 20);        // jump off the wall

    //these public values are there to attach to each of the Child game object within the Player Manager object
    public GameObject char1;
    public GameObject char2;
    public GameObject char3;

    readonly int maxHealth = 3;
    public int damage;

    //hidden values that deal with character movement are in this section
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float timeToJumpApex = .4f;
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    float velocityXSmoothing;
    Vector3 velocity;
    Controller2D controller; //Controller2D handles the actual movement of the transforms, Player class only deals with the physical calculations

    //value to help with character switch
    int selectedChar = 0;

    public int GetCurrHealth()
    {
        return maxHealth - damage;
    }

    public void TakeDamage()
    {
        this.damage += 1;
    }

    public void Heal()
    {
        this.damage -= 1;
    }

    void Start() {
        controller = GetComponent<Controller2D>(); //Attaches a controller2D script to gameobject
        setGravityPhysics();
        ActivateChar1();
        damage = 0;
    }

    private void setGravityPhysics() {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    private void ActivateChar1() {
        selectedChar = 0;
        char1.SetActive(true);
        char2.SetActive(false);
        char3.SetActive(false);
    }

    void Update()
    {
        Vector2 joystickInput = new Vector2(CnInputManager.GetAxisRaw("Horizontal"), CnInputManager.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1; //if facing left, wallDirX = -1, else 1

        setSmoothedVelocityXPhysics(joystickInput);

        bool wallSliding = false;
        if (isFallingAndTouchingWall())
        {
            wallSliding = true; // Set sprites here if wall jumping
            setWallSlidePhysics(joystickInput, wallDirX);
        }

        JumpButtonPressed(wallDirX, wallSliding);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, joystickInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        characterSwapButtonPressed();

        if (isDead())
        {
            Die();
        }
    }

    private bool isDead()
    {
        return GetCurrHealth() <= 0;
    }

    private void JumpButtonPressed(int wallDirX, bool wallSliding) {
        if (CnInputManager.GetButtonDown("Jump")) {
            if (wallSliding) {
                setWallJumpPhysics(wallDirX);

            }
            if (controller.collisions.below) {
                velocity.y = maxJumpVelocity;
            }
        }

        if (CnInputManager.GetButtonUp("Jump")) {
            if (velocity.y > minJumpVelocity) {
                velocity.y = minJumpVelocity;
            }
        }
    }

    private void characterSwapButtonPressed() {
        if (CnInputManager.GetButtonUp("Fire1")) {
            selectedChar = (selectedChar + 1) % 3;
            switch (selectedChar) {
                case 0: //1st char
                    char1.SetActive(true);
                    char2.SetActive(false);
                    char3.SetActive(false);
                    break;
                case 1://2nd char
                    char1.SetActive(false);
                    char2.SetActive(true);
                    char3.SetActive(false);
                    break;
                case 2://last char
                    char1.SetActive(false);
                    char2.SetActive(false);
                    char3.SetActive(true);
                    break;
            }
            //Debug.Log("char1 active = " + char1.activeSelf + "; char2 active = " + char2.activeSelf + "; char3.active = " + char3.activeSelf);
        }
    }

    private void setSmoothedVelocityXPhysics(Vector2 joystickInput) {
        float targetVelocityX = joystickInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(
            velocity.x,
            targetVelocityX,
            ref velocityXSmoothing,
            (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne)
            );
    }

    private void setWallJumpPhysics(int wallDirX) {
        velocity.x = -wallDirX * wallLeap.x;
        velocity.y = wallLeap.y;
    }

    private void setWallSlidePhysics(Vector2 joystickInput, int wallDirX) {
        if (velocity.y < -wallSlideSpeedMax) {
            velocity.y = -wallSlideSpeedMax;
        }
        if (timeToWallUnstick > 0) {
            velocity.x = 0;
            velocityXSmoothing = 0;
            if (joystickInput.x != wallDirX && joystickInput.x != 0) {
                timeToWallUnstick -= Time.deltaTime;
            } else {
                timeToWallUnstick = wallStickTime;
            }
        } else {
            timeToWallUnstick = wallStickTime;
        }
    }

    private bool isFallingAndTouchingWall() {
        return (controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0;
    }

    void Die()
    {
        //Restart
        Application.LoadLevel(Application.loadedLevel);
    }
}
