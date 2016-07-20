//This is free to use and no attribution is required
//No warranty is implied or given
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class LaserBeam : MonoBehaviour {
    LineRenderer line;
    public float distance = 15f;

    bool laserOn;
    public float laserOnTime;
    public float laserOnTimer;

    void Start() {
        line = GetComponent<LineRenderer>();
        laserOn = false;

        laserOn = true;
        laserOnTimer = laserOnTime;
        StartCoroutine(FireLaser());
    }

    void Update() {
        
        
    }

    void laserTimerCd() {
        if (laserOn) {
            if(laserOnTimer > 0) {
                laserOnTimer -= Time.deltaTime;
            } else {
                laserOn = false;
            }
        }
    }

    IEnumerator FireLaser()
    {
        line.enabled = true;
 
        while (laserOn)
        {
            Ray2D ray = new Ray2D(transform.position, transform.right);

            line.SetPosition(0, ray.origin);

            RaycastHit2D[] hits = Physics2D.RaycastAll (ray.origin, Vector2.right, distance);
            bool reachedEnd = false;
            foreach(RaycastHit2D hit in hits) {
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Laser End")) {
                    line.SetPosition(1, hit.point);
                    reachedEnd = true;
                }
                if(hit.collider.CompareTag("Player") || hit.collider.CompareTag("Boss")) {
                    hit.collider.SendMessageUpwards("Damage");
                }
            }
            if (!reachedEnd) {
                line.SetPosition(1, ray.GetPoint(distance));
            }
            laserTimerCd();
            yield return null;
        }
        line.enabled = false;
    }
}
