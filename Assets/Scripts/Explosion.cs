using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour { 
    void OnTriggerEnter2D(Collider2D col) {
        if (!col.isTrigger && col.CompareTag("Player")) {
            col.SendMessageUpwards("Damage");
        }
    }

    public void DestroyThis() {
        Destroy(gameObject);
    }
}
