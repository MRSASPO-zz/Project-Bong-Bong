using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
    Transform target;
    // Use this for initialization

    public float damping = 1f;
    public float speed = 15;
    public float autoExplodeTime = 5f;

    //public GameObject explosion; //prefab for explosion?

	void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //StartCoroutine(AutoExplode());
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 posNormalised = target.position - transform.position; //also the direction
        float angle = Mathf.Atan2(posNormalised.y, posNormalised.x) * 180 / Mathf.PI + 180;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * damping);
        transform.Translate(-speed * Time.deltaTime, 0, 0, Space.Self);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (!col.isTrigger && col.CompareTag("Player")) {
            //col.SendMessageUpwards("Damage");
            ExplodeSelf();
        }
    }

    void ExplodeSelf() {
        //Instantiate(explosion, transform.position, Quarternion.identity);
        Destroy(gameObject);
    }

    IEnumerator AutoExplode() {
        yield return new WaitForSeconds(autoExplodeTime);
        ExplodeSelf();
    }
}
