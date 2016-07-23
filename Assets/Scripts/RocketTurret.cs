using UnityEngine;
using System.Collections;

public class RocketTurret : MonoBehaviour {
    public GameObject projectilePrefab;
    public float firingCd = 1f;
    public Vector3 projectileOffset = new Vector3(0, 1.2f, 0);
    public float range = 10;
    bool firing;
    bool firingReady;
    float firingTimer;
    Transform target;

	void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    void Update () {
        startFiring();
        firingCDTimer();
    }

    public void startFiring() {
        if (firingReady) {
            firingReady = false;
            firingTimer = firingCd;
            Attack();
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

    void Attack() {
        float distance = Vector3.Distance(target.position, transform.position);
        if(distance <= range) {
            Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            spawnPoint = spawnPoint + projectileOffset;
            Quaternion rotation = Quaternion.Euler(0, 0, -90);
            GameObject missile = (GameObject)Instantiate(projectilePrefab, spawnPoint, rotation);
            //missile.transform.parent = transform;
            //Do something to the projectile after spawn
        }
    }
}
