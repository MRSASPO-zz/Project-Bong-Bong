using UnityEngine;
using System.Collections;

public class DangerousObstacle : MonoBehaviour {

    Player player;

	// Use this for initialization
	void Start () {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
    void OmTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.TakeDamage();
        }
    }

}
