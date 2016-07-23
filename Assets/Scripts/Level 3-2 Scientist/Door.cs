using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Door : MonoBehaviour {
    public UnityEvent ue = new UnityEvent();
    public SpriteRenderer sr;
    public Sprite[] images;
    public BoxCollider2D doorCollider;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            closeDoor();
            ue.Invoke();
        }
    }

    public void closeDoor() {
        sr.sprite = images[1];
        doorCollider.gameObject.layer = LayerMask.NameToLayer("Obstacle");
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void openDoor() {
        sr.sprite = images[0];
        doorCollider.gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
