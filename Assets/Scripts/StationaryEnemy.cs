using UnityEngine;
using System.Collections;

public class StationaryEnemy : MonoBehaviour {
    public int health;
    float gravity;

    void Awake() {
        gravity = -(2 * 2.5f) / Mathf.Pow(0.3f, 2); //"max jump height" = 2.5, "time to apex" = 0.3f, equiv values for the player
    }

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
