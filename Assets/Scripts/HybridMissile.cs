using UnityEngine;
using System.Collections;

public class HybridMissile : MonoBehaviour
{
    public float speed;
    private int threshold;
    private int passes = 0;
    public GameObject explosion; //prefab for explosion

    private BoxCollider2D collider;
    private SpriteRenderer sr;
    GameObject AudioSourceGO;
    AudioSource audioSource;
    bool isExploded;
    private float cliplength;

    void Start()
    {
        attachAudioSource();
        collider = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        threshold = Random.Range(0, 2);
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
    void Update()
    {
        transform.Translate(0, -speed*Time.timeScale, 0);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.isTrigger && col.CompareTag("Player"))
        {
            col.SendMessageUpwards("Damage");
            ExplodeSelf();
        }
        else if (!col.isTrigger && col.CompareTag("Floor"))
        {
            if (passes == threshold)
            {
                ExplodeSelf();
            }
            else
            {
                passes += 1;
            }
        }
    }

    void ExplodeSelf() {
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
}