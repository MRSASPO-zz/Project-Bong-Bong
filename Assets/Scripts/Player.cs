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

    readonly int maxHealth = 3;
    public int damage;
    private int poweredUp = 0;

    //Animations
    private Animator anim;
    private Vector3 defaultScale;
    private SpriteRenderer spriteRenderer;

    //hidden values that deal with character movement are in this section
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float timeToJumpApex = .35f;
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    float velocityXSmoothing;
    [HideInInspector]
    public Vector3 velocity;
    Controller2D controller; //Controller2D handles the actual movement of the transforms, Player class only deals with the physical calculations
    bool invulnerable; //Invulnerability period for player
    public float invulTime = 3f;

    //value to help with character switch
    int selectedChar = 0;

    [HideInInspector]
    public bool melee;
    public Collider2D attackTriggerRight;
    public float meleeAttackCooldown;
    private float meeleeAttackTimer;

    private float faceDir;
    private float prevFaceDir;

    private AudioSource audioSource;

    void Awake() {
        controller = GetComponent<Controller2D>(); //Attaches a controller2D script to gameobject
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        setGravityPhysics();
        damage = 0;
        invulnerable = false;
        attackTriggerRight.enabled = false;
        faceDir = 1;
        prevFaceDir = 1;
        defaultScale = transform.localScale;
    }

    private void setGravityPhysics() {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void Update() {
        if(Time.timeScale == 0) {
            return;
        }
        Vector2 joystickInput = new Vector2(CnInputManager.GetAxisRaw("Horizontal"), CnInputManager.GetAxisRaw("Vertical"));

        prevFaceDir = faceDir;
        faceDir = joystickInput.x; //get the input
        if (faceDir == 0) {
            faceDir = prevFaceDir;
        }

        Face(Mathf.Sign(faceDir));

        int wallDirX = (controller.collisions.left) ? -1 : 1; //if facing left, wallDirX = -1, else 1
        setSmoothedVelocityXPhysics(joystickInput);

        //Maru, maru can jump but cannot do anything else
        bool wallSliding = false;
        if (isFallingAndTouchingWall()) {
            wallSliding = true; // Set sprites here if wall jumping
            setWallSlidePhysics(joystickInput, wallDirX);
        }

        JumpButtonPressed(wallDirX, wallSliding);
        MeleeButtonPressed(joystickInput);

        velocity.y += gravity * Time.deltaTime; //apply gravity

        controller.Move(velocity * Time.deltaTime, joystickInput); //move character

        //At the moment no jump animations yet, so considering only the x axis
        //playIdleOrWalk();

        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
        //"Fire1"
        checkAndTriggerDamage();
        checkAndTriggerDeath();
        meleeCDTimer();

        if(!melee && !invulnerable) {
            playIdleOrWalk();
        }
    }

    public void playIdleOrWalk() {
        if (Mathf.Abs(velocity.x) > 0.001) {
            anim.Play("Walking");
        } else {
            anim.Play("Idle");
        }
    }

    void Face(float direction) {
        transform.localScale = new Vector3(defaultScale.x * direction, defaultScale.y, defaultScale.z);
    }

    public bool isPoweredUp()
    {
        return poweredUp>0;
    }

    public void Power()
    {
        //print("WORK NOW");
        poweredUp +=1;
        print(poweredUp);
    }

    public void Depower()
    {
        //print("NO WORK NOW");
        poweredUp -= 1;
        print(poweredUp);
    }

    private void checkAndTriggerDamage() {
        bool isCollideWithDangerousObstacle = controller.collisions.horizontalColliderTag == "Dangerous Obstacle" || controller.collisions.verticalColliderTag == "Dangerous Obstacle";
        if (isCollideWithDangerousObstacle) {
            Damage();
        }
    }

    public void Damage() {
        if (!invulnerable) {
            TakeDamage();
            Knockback();
            StartCoroutine(invulnerability());
            anim.Play("Damaged");
        }
    }

    IEnumerator invulnerability() {
        invulnerable = true;
        Color32 colorOriginal = spriteRenderer.color;
        Color32 faded = colorOriginal;
        faded.a /= 4; //reduces the alpha to give it a faded look
        float startInvulTime = Time.time;
        while((Time.time - startInvulTime) < invulTime) {
            spriteRenderer.color = faded;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = colorOriginal;
            yield return new WaitForSeconds(0.2f);
        }
        invulnerable = false;
    }

    private void checkAndTriggerDeath() {
        bool isCollidingVerticallyWithInvisibleWall = controller.collisions.verticalColliderTag == "Invisible Wall";
        bool isCollidingWithLethalObject = controller.collisions.verticalColliderTag == "Lethal" || controller.collisions.horizontalColliderTag == "Lethal";
        bool isDead = GetCurrHealth() <= 0;
        if (isDead || isCollidingVerticallyWithInvisibleWall || isCollidingWithLethalObject) {
            Die();
        }
    }

    public void Knockback()
    {
        if (controller.collisions.velocityOld.x < 0)
        {
            velocity.x = maxJumpVelocity;
        }
        else if(controller.collisions.velocityOld.x > 0)
        {
            velocity.x = -maxJumpVelocity;
        } 
        if (controller.collisions.velocityOld.y <= 0)
        {
            velocity.y = maxJumpVelocity;
        }
        else
        {
            velocity.y = -maxJumpVelocity;
        }
    }

    public void StrongKnockback()
    {
        if (controller.collisions.velocityOld.x < 0)
        {
            velocity.x = maxJumpVelocity*2;
        }
        else if (controller.collisions.velocityOld.x > 0)
        {
            velocity.x = -maxJumpVelocity*2;
        }
        if (controller.collisions.velocityOld.y <= 0)
        {
            velocity.y = maxJumpVelocity*2;
        }
        else
        {
            velocity.y = -maxJumpVelocity*2;
        }
    }

    //These few functions deal with the several character based button inputs, the button used here is "Jump" but the character may not jump if its not char1 etc.
    private void JumpButtonPressed(int wallDirX, bool wallSliding) {
        if (CnInputManager.GetButtonDown("Jump")) {
            if (wallSliding) {
                print(controller.collisions.horizontalColliderTag);
                if (controller.collisions.horizontalColliderTag == "WallJumpable") {
                    setWallJumpPhysics(wallDirX);
                }
            }
            if (controller.collisions.below) {
                audioSource.clip = AudioManager.audioClips["Jump Sound"];
                audioSource.Play();
                velocity.y = maxJumpVelocity;
            }
        }

        if (CnInputManager.GetButtonUp("Jump")) {
            if (velocity.y > minJumpVelocity) {
                velocity.y = minJumpVelocity;
            }
        }
    }

    private void MeleeButtonPressed(Vector2 joyStickInput) {
        if (CnInputManager.GetButtonDown("Fire1") && !melee) {
            melee = true;
            meeleeAttackTimer = meleeAttackCooldown;
            audioSource.clip = AudioManager.audioClips["Punch Sound"];
            audioSource.Play();
            attackTriggerRight.enabled = true;
            if (isPoweredUp()) {
                anim.Play("Power Attack");
            } else {
                anim.Play("Attacking");
            }
        }
    }

    private void meleeCDTimer() {
        if (melee) {
            if (meeleeAttackTimer > 0) {
                meeleeAttackTimer -= Time.deltaTime;
            } else {
                melee = false;
                attackTriggerRight.enabled = false;
            }
        } 
    }

    private void setSmoothedVelocityXPhysics(Vector2 joystickInput) {
        float targetVelocityX;
        targetVelocityX = joystickInput.x * moveSpeed;
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
}
