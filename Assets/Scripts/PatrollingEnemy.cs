using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class PatrollingEnemy : MonoBehaviour {
    public LayerMask enemyCollisionMask;
    public LayerMask wallMask;
    public float speed = 3;

    Controller2D controller;
    SpriteRenderer image;
    float gravity;
    float directionX; //assume only moving on platform
    Vector3 velocity;
    Bounds colliderBounds;
    void Start() {
        controller = GetComponent<Controller2D>();
        image = GetComponent<SpriteRenderer>();
        //image.flipX = !image.flipX;
        gravity = -(2 * 2.5f) / Mathf.Pow(0.3f, 2); //"max jump height" = 2.5, "time to apex" = 0.3f, equiv values for the player
        directionX = -1; //initially moving leftwards
    }

    void Update() {
        Vector2 rayOrigin = (directionX == 1) ? controller.raycastOrigins.bottomRight : controller.raycastOrigins.bottomLeft;
        float rayLength = 2*.015f;
        RaycastHit2D collisionHit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, enemyCollisionMask);
        RaycastHit2D wallHit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, wallMask);

        if (collisionHit ||wallHit) {
            directionX = directionX * -1;
            image.flipX = !image.flipX;
        }
        velocity.x = directionX * speed;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity*Time.deltaTime, Vector2.zero);
        //transform.Translate();
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
    }
    //Deals damage to player
    void OnTriggerStay2D(Collider2D col) {
        if (!col.isTrigger && col.CompareTag("Player")) {
            col.SendMessageUpwards("Damage");
        }
    }
}
