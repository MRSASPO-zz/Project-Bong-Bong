using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
    Transform target;
    // Use this for initialization

    public float damping = 1f;
    public float speed = 15;
    public float autoExplodeTime = 5f;

    public GameObject explosion; //prefab for explosion
    private BoxCollider2D collider;
    private SpriteRenderer sr;
    private float cliplength;
    private bool isExploded;

    GameObject AudioSourceGO;
    AudioSource audioSource;

    void Start () {
        collider = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        attachAudioSource();
        StartCoroutine(AutoExplode());
        isExploded = false;
    }

    private void attachAudioSource() {
        this.AudioSourceGO = ObjectPoolManager.Instance.GetObject("AudioSourcePrefab");
        this.audioSource = this.AudioSourceGO.GetComponent<AudioSource>();
        this.audioSource.playOnAwake = false;
        this.audioSource.maxDistance = 15;
        this.audioSource.clip = AudioManager.audioClips["Missile Explosion Sound"];
        this.cliplength = audioSource.clip.length;
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Custom;
        this.AudioSourceGO.SetActive(true);
    }

    private void detachAudioSource() {
        audioSource = null;
        AudioSourceGO.SetActive(false);
        AudioSourceGO = null;
    }

    // Update is called once per frame
    void Update () {
        Vector3 posNormalised = target.position - transform.position; //also the direction
        float angle = Mathf.Atan2(posNormalised.y, posNormalised.x) * 180 / Mathf.PI + 180;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * damping);
        transform.Translate(-speed * Time.deltaTime, 0, 0, Space.Self);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (!col.isTrigger && col.CompareTag("Player")) {
            //col.SendMessageUpwards("Damage");
            ExplodeSelf();
        }
    }

    public void ExplodeSelf() {
        collider.enabled = false;
        sr.enabled = false;

        if (!isExploded) {
            isExploded = true;
            AudioSourceGO.transform.position = this.transform.position;
            audioSource.Play();
            Instantiate(explosion, transform.position, Quaternion.identity);
            StartCoroutine(detachAudioSourceAndDestroy());
        }
    }
        
    IEnumerator detachAudioSourceAndDestroy() {
        yield return new WaitForSeconds(cliplength);
        detachAudioSource();
        Destroy(gameObject);
    }

    IEnumerator AutoExplode() {
        yield return new WaitForSeconds(autoExplodeTime);
        ExplodeSelf();
    }
}
