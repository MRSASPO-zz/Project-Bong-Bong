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
    public LayerMask endMask;

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
    public bool isFireForever;
    //public float cooldownTime;
    //public bool isCoolingDown;
    //float cooldownTimer;

    private GameObject AudioSourceGO;
    private AudioSource audioSource;

    void Awake() {
        line = GetComponent<LineRenderer>();
        directionDictionary.Add(0, Vector2.right);
        directionDictionary.Add(1, Vector2.down);
        directionDictionary.Add(2, Vector2.left);
        directionDictionary.Add(3, Vector2.up);
        laserOn = false;

        //isCoolingDown = false;
    }

    void Start() {
        attachAudioSource();
        if (isFireForever) {
            shootLaser();
        }
    }

    private void attachAudioSource() {
        this.AudioSourceGO = ObjectPoolManager.Instance.GetObject("AudioSourcePrefab");
        this.audioSource = this.AudioSourceGO.GetComponent<AudioSource>();
        this.audioSource.maxDistance = 15;
        this.audioSource.clip = AudioManager.audioClips["Laser Sound"];
        audioSource.rolloffMode = AudioRolloffMode.Custom;
        audioSource.spatialBlend = 1;
        audioSource.loop = true;
        this.AudioSourceGO.SetActive(true);
    }

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
        audioSource.Play();
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
            if(!isFireForever) {
                if (laserOnTimer > 0) {
                    laserOnTimer -= Time.deltaTime;
                } else {
                    audioSource.loop = false;
                    laserOn = false;
                }
            }
        }
    }

    IEnumerator FireLaser()
    {
        spriteRenderer.sprite = images[1];
        line.enabled = true;
        while (laserOn)
        {
            AudioSourceGO.transform.position = transform.position;
            Ray2D ray = new Ray2D(Vector3.zero, directionDictionary[direction]);

            line.material.mainTextureOffset = new Vector2(Time.time*10, Time.time*10);
            line.SetPosition(0, ray.origin);
            RaycastHit2D hitEnd = Physics2D.Raycast(transform.position, directionDictionary[direction], distance, endMask);
            float travelled = distance;
            if (hitEnd) {
                if (direction == 0 || direction == 2) {
                    travelled = hitEnd.point.x - transform.position.x;
                    line.SetPosition(1, new Vector3(travelled, 0, 0));
                } else if (direction == 1 || direction == 3) {
                    travelled = hitEnd.point.y - transform.position.y;
                    line.SetPosition(1, new Vector3(0, travelled, 0));
                }
            } else {
                line.SetPosition(1, ray.GetPoint(distance));
            }

            RaycastHit2D[] hits = Physics2D.RaycastAll (transform.position, directionDictionary[direction], Mathf.Abs(travelled));
            //Debug.DrawRay(transform.position, directionDictionary[direction]* Mathf.Abs(travelled), Color.cyan);
            foreach(RaycastHit2D hit in hits) {
                
                if(hit.collider.CompareTag("Player")) {
                    hit.collider.SendMessageUpwards("Damage");
                }
                if (hit.collider.CompareTag("Boss")) {
                    hit.collider.SendMessageUpwards("LaserDamage");
                }
                if (isFireForever) {
                    if (hit.collider.CompareTag("Destroyable")) {
                        hit.collider.SendMessageUpwards("ExplodeSelf");
                    }
                }
            }
            laserTimerFiring();
            yield return null;
        }
        line.enabled = false;
    }
}
