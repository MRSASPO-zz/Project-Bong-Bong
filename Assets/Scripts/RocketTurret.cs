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
    private AudioSource audioSource;

	void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
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
            audioSource.clip = AudioManager.audioClips["Gunshot Sound"];
            audioSource.Play();
            GameObject missile = (GameObject)Instantiate(projectilePrefab, spawnPoint, rotation);
        }
    }
}
