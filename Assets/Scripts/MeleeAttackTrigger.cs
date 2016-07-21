using UnityEngine;
using System.Collections;

public class MeleeAttackTrigger : MonoBehaviour {
    public int damage = 1;
    public Player player;
	
    void OnTriggerEnter2D(Collider2D col) {
        //!col.isTrigger && 
        if (col.CompareTag("Melee Enemy")) {
            col.SendMessageUpwards("Damage", damage);
        }
        if (col.CompareTag("Boss"))
        {
            if (player.isPoweredUp())
            {
                col.SendMessageUpwards("Damage");
                player.Depower();
                player.Knockback();
            }
        }
        if (col.CompareTag("Lever")) {
            col.SendMessageUpwards("PushLever");
        }
    }
}
