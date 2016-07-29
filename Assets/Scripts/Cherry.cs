using UnityEngine;
using System.Collections;

public class Cherry : MonoBehaviour {
    GameObject AudioSourceGO;
    AudioSource audioSource;

    void Start() {
        attachAudioSource();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            AudioSourceGO.transform.position = transform.position;
            audioSource.Play();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            col.SendMessageUpwards("Power");
            StartCoroutine(detachAudioSourceAndDestroy());
        }
    }

    IEnumerator detachAudioSourceAndDestroy() {
        yield return new WaitForSeconds(audioSource.clip.length*2);
        detachAudioSource();
        Destroy(gameObject);
    }

    private void attachAudioSource() {
        this.AudioSourceGO = ObjectPoolManager.Instance.GetObject("AudioSourcePrefab");
        this.audioSource = this.AudioSourceGO.GetComponent<AudioSource>();
        this.audioSource.playOnAwake = false;
        this.audioSource.maxDistance = 15;
        this.audioSource.clip = AudioManager.audioClips["Powerup Pickup Sound"];
        audioSource.spatialBlend = 0;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        this.AudioSourceGO.SetActive(true);
    }

    private void detachAudioSource() {
        audioSource = null;
        AudioSourceGO.SetActive(false);
        AudioSourceGO = null;
    }
}
