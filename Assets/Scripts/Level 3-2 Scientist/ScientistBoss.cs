using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ScientistBoss : Boss {
    //char specific var
    Controller2D controller;
    Animator anim;
    Transform playerT; //Player Transform, to change direction of face
    int faceDir; //the direction of which the boss is facing
    Vector3 defaultScale;
    float gravity;
    Vector3 velocity;
    BoxCollider2D collider;
    SpriteRenderer spriteRenderer;
    private bool invul;
    //3 states, attack, teleporting, idle

    public UnityEvent ue = new UnityEvent();
    public int health;

    //Attacking var
    public float missileFireRate = 1f;
    public int numberOfMissiles = 3;
    public GameObject projectilePrefab; //Missile
    public Vector3 projectileOffset; //To be set
    public float firingCd;

    private bool firing;
    private bool firingReady;
    private float firingTimer;

    //Teleport var
    public Vector3[] localWayPoints;
    public float teleportCd;

    private Vector3[] globalWayPoints;
    private bool teleporting;
    private bool teleportReady;
    private float teleportingTimer;
    private System.Random rnd = new System.Random();
    private int positionIndex; //denotes the current position (NOTE that you should keep the 1st point of localWayPoint as (0, 0, 0) for it to work
                               //
    private AudioSource audioSource;
    
    void Start () {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * 2.5f) / Mathf.Pow(0.3f, 2); //"max jump height" = 2.5, "time to apex" = 0.3f, equiv values for the player
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        defaultScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        setTeleportPoints();
        firingReady = true;
        teleportReady = false;
        teleportingTimer = teleportCd;
        positionIndex = 0;
    }

    private void setTeleportPoints() {
        globalWayPoints = new Vector3[localWayPoints.Length];
        for (int i = 0; i < localWayPoints.Length; i++) {
            globalWayPoints[i] = localWayPoints[i] + transform.position;
        }
    }

    void Update () {
        if(health <= 0) {
            ue.Invoke();
            Destroy(gameObject);
            return;
        }
        faceDir = ((this.transform.position.x - playerT.position.x) > 0) ? -1 : 1;
        Face(faceDir);

        startFiring();
        firingCDTimer();

        beginTeleport();
        TeleportCdTimer();

        DropBoss(); //Unrelated to boss mechanics
    }

    //boss only falls, doesn't move left or right
    private void DropBoss() {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, Vector2.zero);
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
    }

    void Face(int direction) {
        transform.localScale = new Vector3(defaultScale.x * direction* -1, defaultScale.y, defaultScale.z);
    }

    IEnumerator Attack() {
        firing = true;
        for (int i=0; i<numberOfMissiles; i++) {

            Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Quaternion rotation = new Quaternion(); //flip about x or y axis
            if (faceDir > 0) {
                //offset in the x direction (reflection about y axis)
                Vector3 offset = projectileOffset;
                offset.x = -offset.x;
                spawnPoint = spawnPoint + offset;
                rotation = Quaternion.Euler(0, 0, 180);
            } else if (faceDir < 0) {
                spawnPoint = spawnPoint + projectileOffset;
                rotation = Quaternion.identity;
            }
            
            anim.Play("Firing", -1, 0f);
            anim.Play("Firing");
            audioSource.clip = AudioManager.audioClips["Gunshot Sound"];
            audioSource.Play();
            GameObject missile = (GameObject)Instantiate(projectilePrefab, spawnPoint, rotation);
            //missile.transform.parent = transform;
            //Do something to the projectile after spawn
            yield return new WaitForSeconds(missileFireRate);
        }
        anim.Play("Walking Or Idle");
        firing = false;
    }

    public void startFiring() {
        if (firingReady && !teleporting) {
            firingReady = false;
            firingTimer = firingCd;
            StartCoroutine(Attack());
        }
    }

    private void firingCDTimer() {
        if (!firingReady) {
            if (firingTimer > 0) {
                firingTimer -= Time.deltaTime;
            } else {
                firingReady = true;
            }
        }
    }

    void beginTeleport() {
        if (!firing && teleportReady) {
            teleporting = true;
            teleportReady = false;
            teleportingTimer = teleportCd;
            anim.Play("Teleporting");
        }
    }

    void Teleport() {
        int rndInt = rnd.Next(0, 4);
        while(positionIndex == rndInt) {
            rndInt = rnd.Next(0, 4);
        }
        positionIndex = rndInt;
        transform.position = globalWayPoints[positionIndex];
        anim.Play("Reappear");
        collider.isTrigger = true;
    }

    void TeleportEnd() {
        teleporting = false;
        collider.isTrigger = false;
        anim.Play("Walking Or Idle");
    }

    private void TeleportCdTimer() {
        if (!teleportReady) {
            if (teleportingTimer > 0) {
                teleportingTimer -= Time.deltaTime;
            } else {
                teleportReady = true;
            }
        }
    }

    public void Damage() {
        if (!invul) {
            health--;
            StartCoroutine(invulnerability());
        }
    }

    public void LaserDamage() {
        if (!invul) {
            health-= 5;
            StartCoroutine(invulnerability());
        }
    }

    override public int getBossHealth() {
        if(health <= 0) {
            return 0;
        } else {
            float hp = (float)health;
            hp /= 10;
            return Mathf.CeilToInt(hp);
        }
    }

    IEnumerator invulnerability() {
        invul = true;
        Color32 colorOriginal = spriteRenderer.color;
        Color32 faded = colorOriginal;
        faded.a /= 4; //reduces the alpha to give it a faded look
        float startInvulTime = Time.time;
        while ((Time.time - startInvulTime) < 3) {
            spriteRenderer.color = faded;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = colorOriginal;
            yield return new WaitForSeconds(0.2f);
        }
        invul = false;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            col.SendMessageUpwards("Knockback");
        }
    }

    void OnDrawGizmos() {
        if (localWayPoints != null) {
            Gizmos.color = Color.red;
            float size = .3f;

            for (int i = 0; i < localWayPoints.Length; i++) {
                Vector3 globalWaypointPos = (Application.isPlaying) ? globalWayPoints[i] : localWayPoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }
}
