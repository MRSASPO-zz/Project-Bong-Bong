using UnityEngine;
using System.Collections;

public class HybridMissile : MonoBehaviour
{
    public float speed;
    private int threshold;
    private int passes = 0;
    public GameObject explosion; //prefab for explosion

    void Start()
    {
        threshold = Random.Range(0, 2);
    }

    // Update is called once per frame


    void Update()
    {
        transform.Translate(0, -speed*Time.timeScale, 0);
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
            if (passes == threshold)
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
        Destroy(gameObject);
    }

}