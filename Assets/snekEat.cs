using UnityEngine;
using System.Collections;

public class snekEat : MonoBehaviour {
    public Vector3 heightToJump;
    public Vector3 targetPosition;
    public Vector3 originalPosition;
    private SpriteRenderer sr;
    bool isUp;
    // Use this for initialization
    void Start () {
        targetPosition = transform.position + heightToJump;
        originalPosition = transform.position;
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(targetPosition, transform.position);
        float sign = Mathf.Sign(targetPosition.y - transform.position.y);
	    if((distance* sign) > 0) {
            sr.flipX = false;
            transform.Translate(heightToJump * Time.deltaTime, Space.World);
        } else {
            isUp = false;
            sr.flipX = true;
            transform.Translate(-heightToJump * Time.deltaTime, Space.World);
        }
	}
}
