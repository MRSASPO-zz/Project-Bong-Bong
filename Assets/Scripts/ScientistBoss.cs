using UnityEngine;
using System.Collections;

public class ScientistBoss : MonoBehaviour {
    Animator anim;
    //Player Transform, to change direction of face
    Transform playerT;
    int faceDir; //the direction of which the boss is facing
    Vector3 defaultScale;

    //3 states, attack, teleporting, idle

    //Attacking var
    public float missileFireRate = 1f;
    public GameObject projectilePrefab; //Missile
    public int numberOfMissiles = 3;
    public Vector3 projectileOffset; //To be set
    private bool firing;
    public float firingCd;
    private float firingTimer;

    void Start () {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        defaultScale = transform.localScale;
	}
	
	void Update () {
        faceDir = ((this.transform.position.x - playerT.position.x) > 0) ? -1 : 1;
        startFiring();
        firingCDTimer();
    }

    IEnumerator Attack() {
        Vector3 spawnPoint = new Vector3(transform.position.x,
            transform.position.y,
            transform.position.z);
        //if (faceDir > 0) {

        //} else if (faceDir < 0) {

        //}
        //spawnPoint = spawnPoint + projectileOffset;
        
        for (int i=0; i<numberOfMissiles; i++) {
            anim.Play("Firing");
            Instantiate(projectilePrefab, spawnPoint, Quaternion.identity);
            print("i: " + i);
            //Do something to the projectile after spawn
            yield return new WaitForSeconds(missileFireRate);
        }
        print("end");
        anim.Play("Walking Or Idle");
    }

    private void firingCDTimer() {
        if (firing) {
            if (firingTimer > 0) {
                firingTimer -= Time.deltaTime;
            } else {
                firing = false;
            }
        }
    }

    private void startFiring() {
        if (!firing) {
            firing = true;
            firingTimer = firingCd;
            StartCoroutine(Attack());
        }
    }

    void Teleport() {

    }

    void Idle() {

    }
}
