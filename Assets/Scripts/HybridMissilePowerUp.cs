using UnityEngine;
using System.Collections;

public class HybridMissilePowerUp : MonoBehaviour {


    public float speed;
    public float autoExplodeTime = 2f;
    public GameObject powerup;
    public GameObject explosion; //prefab for explosion

        void Start()
        {
            StartCoroutine(AutoExplode());
        }

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
    }

    void ExplodeSelf()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        GameObject PU = (GameObject)Instantiate(powerup, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator AutoExplode()
    {
        yield return new WaitForSeconds(autoExplodeTime);
        ExplodeSelf();
    }
}