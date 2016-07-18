using UnityEngine;
using System.Collections;

public class RisingLava : MonoBehaviour {
    public float speed = .1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        speed += .035f;
        transform.position = new Vector3(transform.position.x, speed, transform.position.z);
    }
}
