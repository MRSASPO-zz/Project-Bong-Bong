//This is free to use and no attribution is required
//No warranty is implied or given
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]

public class LaserBeam : MonoBehaviour {
    LineRenderer line;
    public SpriteRenderer spriteRenderer;
    public Sprite[] images;
    public float distance = 150f;

    [Range(0, 3)]
    // 0 => right
    // 1 => down
    // 2 => left
    // 3 => up
    public int direction;
    Dictionary<int, Vector3> directionDictionary = new Dictionary<int, Vector3>();

    public float laserOnTime;
    
    bool laserOn;
    
    float laserOnTimer;
    
    public bool activated;

    //public float cooldownTime;
    //public bool isCoolingDown;
    //float cooldownTimer;

    void Awake() {
        line = GetComponent<LineRenderer>();
        directionDictionary.Add(0, Vector2.right);
        directionDictionary.Add(1, Vector2.down);
        directionDictionary.Add(2, Vector2.left);
        directionDictionary.Add(3, Vector2.up);
        laserOn = false;

        //isCoolingDown = false;
    }

    //void Update() {
        //shootLaser();
        //firingCDTimer();
    //}

    public void shootLaser() {
        if (activated) {
            spriteRenderer.sprite = images[0];
            startFiring();
        } else {
            spriteRenderer.sprite = images[2];
        }
    }

    public void refresh(bool isCooldown) {
        if (activated) {
            if (isCooldown) {
                spriteRenderer.sprite = images[1];
            } else {
                spriteRenderer.sprite = images[0];
            }
        } else {
            spriteRenderer.sprite =images[2];
        }
    }

    private void startFiring() {
        laserOn = true;
        laserOnTimer = laserOnTime;
        StartCoroutine(FireLaser());

        //if (!isCoolingDown) {
           // isCoolingDown = true;
         //   cooldownTimer = cooldownTime;
       // }
    }

    //private void firingCDTimer() {
      //  if (isCoolingDown) {
        //    if (cooldownTimer > 0) {
          //      cooldownTimer -= Time.deltaTime;
            //} else {
              //  isCoolingDown = false;
                //if (activated) {
                  //  spriteRenderer.sprite = images[0];
                //}
            //}
        //}
    //}

    void laserTimerFiring() {
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
        spriteRenderer.sprite = images[1];
        line.enabled = true;
 
        while (laserOn)
        {   
            Ray2D ray = new Ray2D(Vector3.zero, directionDictionary[direction]);

            line.material.mainTextureOffset = new Vector2(Time.time*10, Time.time*10);
            line.SetPosition(0, ray.origin);
            
            RaycastHit2D[] hits = Physics2D.RaycastAll (transform.position, directionDictionary[direction], distance);
            bool reachedEnd = false;
            foreach(RaycastHit2D hit in hits) {
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Laser End")) {
                    if (direction == 0 || direction == 2) {
                        float travelled = hit.point.x - transform.position.x;
                        line.SetPosition(1, new Vector3(travelled, 0, 0));
                    } else if (direction == 1 || direction == 3) {
                        float travelled = hit.point.y - transform.position.y;
                        line.SetPosition(1, new Vector3(0, travelled, 0));
                    }
                    
                    reachedEnd = true;
                }
                if(hit.collider.CompareTag("Player")) {
                    hit.collider.SendMessageUpwards("Damage");
                }
                if (hit.collider.CompareTag("Boss")) {
                    hit.collider.SendMessageUpwards("LaserDamage");
                }
                if (hit.collider.CompareTag("Destroyable")) {
                    hit.collider.SendMessageUpwards("ExplodeSelf");
                }
            }
            if (!reachedEnd) {
                line.SetPosition(1, ray.GetPoint(distance));
            }
            laserTimerFiring();
            yield return null;
        }
        line.enabled = false;
    }
}
