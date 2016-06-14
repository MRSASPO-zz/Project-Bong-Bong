using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class PatrollingEnemy : MonoBehaviour {
    public LayerMask enemyMask;
    public float speed = 3;

    Controller2D controller;
    Transform controllerTransform;
    float width, height;
    float gravity;
    float directionX; //assume only moving on platform
    Vector3 velocity;
    Bounds colliderBounds;
    void Start() {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * 2.5f) / Mathf.Pow(0.3f, 2); //"max jump height" = 2.5, "time to apex" = 0.3f, equiv values for the player
        directionX = 1; //initially moving rightwards
        controllerTransform = this.transform;
        
    }

    void Update() {
        colliderBounds = GetComponent<BoxCollider2D>().bounds;
        Vector2 linecastPosLeft = new Vector2(colliderBounds.min.x, colliderBounds.min.y);
        Vector2 linecastPosRight = new Vector2(colliderBounds.max.x, colliderBounds.min.y);
        bool isGrounded = Physics2D.Linecast(linecastPosLeft, linecastPosLeft + Vector2.down, enemyMask);


        velocity.x = directionX * speed;
        velocity.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime, Vector2.zero);

        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
    }

}
