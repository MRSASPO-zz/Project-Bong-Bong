using UnityEngine;
using System.Collections;

public class MeleeAttackTrigger : MonoBehaviour {
    public int damage = 1;
	
    void OnTriggerEnter2D(Collider2D col) {
        if(!col.isTrigger && col.CompareTag("Melee Enemy")) {
            col.SendMessageUpwards("Damage", damage);
        }
    }
}
