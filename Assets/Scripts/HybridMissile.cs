using UnityEngine;
using System.Collections;

public class HybridMissile : MonoBehaviour
{
    public float speed;
    public float autoExplodeTime = 2f;

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
        Destroy(gameObject);
    }

    IEnumerator AutoExplode()
    {
        yield return new WaitForSeconds(autoExplodeTime);
        ExplodeSelf();
    }
}