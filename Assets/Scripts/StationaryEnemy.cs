using UnityEngine;
using System.Collections;

public class StationaryEnemy : MonoBehaviour {
    public int health;
    
    void Update() {
        if(health <= 0) {
            Destroy(gameObject);
        }
    }

    public void Damage(int dmg) {
        health -= dmg;
        print("health: " + health);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (!col.isTrigger && col.CompareTag("Player")) {
            col.SendMessageUpwards("Damage");
        }
    }
}
