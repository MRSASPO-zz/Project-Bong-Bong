using UnityEngine;
using System.Collections;

public class HybridMissilePowerUp : MonoBehaviour {


    public float speed;
    public int threshold;
    private int passes = 0;
    public GameObject powerup;
    public GameObject explosion; //prefab for explosion

        // Update is called once per frame
        void Update()
        {
            transform.Translate(0, -speed, 0);
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
            if (passes==threshold)
            {
                ExplodeSelf();
            }
            else
            {
                passes += 1;
            }
        }
    }

    void ExplodeSelf()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        GameObject PU = (GameObject)Instantiate(powerup, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}