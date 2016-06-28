using UnityEngine;
using System.Collections;

public class CameraMovementL14 : MonoBehaviour {
    public float speed;
    public Controller2D target; //Only used to track the y axis of player
    public float max_x;

    float targetVelocityX;
    Vector3 velocity;

	// Use this for initialization
	void Start () {

    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, target.transform.position.y, transform.position.z), speed * Time.deltaTime * 100);
        transform.Translate(new Vector3(speed*Time.deltaTime, 0, 0));
        float max_pos_x = Mathf.Clamp(transform.position.x, -100, max_x);
        transform.position = new Vector3(max_pos_x, transform.position.y, transform.position.z);
    }

}
