using UnityEngine;
using System.Collections;
using CnControls;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {
    /**
        These values are public and can be changed at any moment if 
        the default values arent desired through the editor
    **/
    //public Values that deal with character movement are in this section
    public float moveSpeed = 10;
    public float maxJumpHeight = 2.5f;
    public float minJumpHeight = 1.1f;
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
    float timeToJumpApex = .3f;
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    float velocityXSmoothing;
    [HideInInspector]
    public Vector3 velocity;
    Controller2D controller; //Controller2D handles the actual movement of the transforms, Player class only deals with the physical calculations
    bool invulnerable; //Invulnerability period for player

    //These 3 of the collisionInfo that is set by the moving platform controller, the values will be reset upon when this script has its own run method
    int pushedByPlatform;
    string horizontalCollisionTag;

    //value to help with character switch
    int selectedChar = 0;

    public int GetCurrHealth() {
        return maxHealth - damage;
    }

    public void TakeDamage() {
        this.damage += 1;
    }

    public void TakeLethalDamage() {
        this.damage = this.maxHealth;
    }

    public void Heal() {
        this.damage -= 1;
    }

    void Start() {
        controller = GetComponent<Controller2D>(); //Attaches a controller2D script to gameobject
        setGravityPhysics();
        ActivateChar1();
        damage = 0;
        invulnerable = false;
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

    void Update() {
        Vector2 joystickInput = new Vector2(CnInputManager.GetAxisRaw("Horizontal"), CnInputManager.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1; //if facing left, wallDirX = -1, else 1

        pushedByPlatform = controller.pushedByPlatform;
        horizontalCollisionTag = controller.platformTag;

        setSmoothedVelocityXPhysics(joystickInput);

        bool wallSliding = false;
        if (isFallingAndTouchingWall()) {
            wallSliding = true; // Set sprites here if wall jumping
            setWallSlidePhysics(joystickInput, wallDirX);
        }

        JumpButtonPressed(wallDirX, wallSliding);

        velocity.y += gravity * Time.deltaTime; //apply gravity
        controller.Move(velocity * Time.deltaTime, joystickInput, pushedByPlatform, horizontalCollisionTag); //move character
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }

        characterSwapButtonPressed();

        if (collideWithDangerousObstacle())
        {
            if (!invulnerable)
            {
                TakeDamage();
                Knockback();
                invulnerability();
                Invoke("resetInvulnerability", 3.0f);
            }
        }

        if (controller.collisions.verticalMovementTag == "Melee Enemy" || controller.collisions.horizontalMovementTag == "Melee Enemy") {
            if (!invulnerable) {
                TakeDamage();
                Knockback();
                invulnerability();
                Invoke("resetInvulnerability", 3.0f);
            }
        }

        if (isDead())
        {
            Die();
        }
        
        if(controller.collisions.verticalColliderTag == "Invisible Wall") {
            Die();
        }
    }

    private void Knockback()
    {
        if (controller.collisions.velocityOld.x < 0)
        {
            velocity.x = maxJumpVelocity/2;
        }
        else if(controller.collisions.velocityOld.x > 0)
        {
            velocity.x = -maxJumpVelocity/2;
        } 
        if (controller.collisions.velocityOld.y <= 0)
        {
            velocity.y = maxJumpVelocity/2;
        }
        else
        {
            velocity.y = -maxJumpVelocity/2;
        }
    }

    private bool collideWithDangerousObstacle()
    {
        return controller.collisions.horizontalColliderTag == "Dangerous Obstacle" || controller.collisions.verticalColliderTag == "Dangerous Obstacle";
    }

    private bool isDead()
    {
        return GetCurrHealth() <= 0;
    }

    public void invulnerability()
    {
        invulnerable = true;
    }

    public void resetInvulnerability()
    {
        invulnerable = false;
    }

    private void JumpButtonPressed(int wallDirX, bool wallSliding) {
        if (CnInputManager.GetButtonDown("Jump")) {
            if (wallSliding) {
                print(controller.collisions.horizontalColliderTag);
                if(controller.collisions.horizontalColliderTag == "WallJumpable") {
                    setWallJumpPhysics(wallDirX);
                }
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
            if (controller.collisions.below) {
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
        bool isLeftRightCollide = (controller.collisions.left || controller.collisions.right);
        bool isFallingAndMidAir = !controller.collisions.below && velocity.y < 0;
        bool isNotInvisibleWall = !(controller.collisions.horizontalColliderTag == "Invisible Wall");
        bool isWallJump = (controller.collisions.horizontalColliderTag == "WallJumpable");
        return isLeftRightCollide && isFallingAndMidAir && isNotInvisibleWall && isWallJump;
    }

    void Die()
    {
        //Restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
