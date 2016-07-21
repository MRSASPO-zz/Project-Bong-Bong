using UnityEngine;
using System.Collections;

public class SpawnPowerUp : MonoBehaviour {

    public GameObject powerup;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Melee Enemy"))
        {
            //print("COLLIDED");
            GameObject PU = (GameObject)Instantiate(powerup, col.transform.position, Quaternion.identity);
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
