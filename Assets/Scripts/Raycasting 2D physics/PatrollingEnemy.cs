﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class PatrollingEnemy : MonoBehaviour {
    public LayerMask enemyCollisionMask;
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
        Vector2 rayOrigin = (directionX == -1) ? controller.raycastOrigins.bottomLeft : controller.raycastOrigins.bottomRight;
        float rayLength = 2*.015f;
        RaycastHit2D collisionHit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, enemyCollisionMask);
        Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.blue);

        if (collisionHit) {
            directionX = directionX / -1;
        }
        velocity.x = directionX * speed;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity*Time.deltaTime, Vector2.zero);
        //transform.Translate();
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
    }

}
