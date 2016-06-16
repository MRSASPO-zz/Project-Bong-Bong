using UnityEngine;
using System.Collections;

public class Controller2D : RayCastController {
    float maxClimbAngle = 80;
    float maxDescendAngle = 75;

    public CollisionInfo collisions;
    [HideInInspector]
    public Vector2 playerInput;

    public override void Start() {
        base.Start();
        collisions.faceDirection = 1;
    }

    //Wrapper for platform movements
    public void Move(Vector3 velocity, bool standingOnPlatform) {
        Move(velocity, Vector2.zero, standingOnPlatform);
    }

    public void Move(Vector3 velocity, Vector2 input, bool standingOnPlatform = false) {
        UpdateRaycastOrigin();
        collisions.Reset();
        collisions.velocityOld = velocity;
        playerInput = input;

        if(velocity.x != 0) {
            collisions.faceDirection = (int)Mathf.Sign(velocity.x);
        }

        if(velocity.y < 0) {
            DescendSlope(ref velocity);
        }

        HorizontalCollisions(ref velocity);

        if (velocity.y != 0) {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);

        if (standingOnPlatform) {
            collisions.below = true;
        }
    }

    void HorizontalCollisions(ref Vector3 velocity) {
        float directionX = collisions.faceDirection;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if(Mathf.Abs(velocity.x) < skinWidth) {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D collisionHit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            RaycastHit2D movementHit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, movingObjectsMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (collisionHit) {

                if(collisionHit.distance == 0) {
                    continue;
                }

                float slopeAngle = Vector2.Angle(collisionHit.normal, Vector2.up);
                if (i == 0 && slopeAngle <= maxClimbAngle) {
                    if (collisions.descendingSlope) {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    if(slopeAngle != collisions.slopeAngleOld) {
                        distanceToSlopeStart = collisionHit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }
                //not climbing slope
                if(!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
                    velocity.x = (collisionHit.distance - skinWidth) * directionX;
                    rayLength = collisionHit.distance;

                    if (collisions.climbingSlope) {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad * Mathf.Abs(velocity.x));
                    }

                    collisions.left = (directionX == -1); //If hit something and going left, then set to true
                    collisions.right = (directionX == 1); //else set right to true
                    collisions.horizontalColliderTag = collisionHit.collider.tag; //Check if the tag is the invisiblewall
                }
            }
            if (movementHit) {
                collisions.horizontalMovementTag = movementHit.collider.tag;
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity) {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D collisionHit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            RaycastHit2D movementHit = Physics2D.Raycast(rayOrigin, Vector2.right * directionY, rayLength, movingObjectsMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if(collisionHit) {
                if(collisionHit.collider.tag == "Through") {
                    if (directionY == 1 || collisionHit.distance == 0) {
                        continue;
                    }
                    /*if (collisions.fallingThroughPlatform) {
                        continue;
                    }
                    if (playerInput.y == -1) {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", .2f); //sets the time for another fall thru
                        continue;
                    }*/
                }

                velocity.y = (collisionHit.distance - skinWidth)* directionY;
                rayLength = collisionHit.distance;

                if (collisions.climbingSlope) {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.below = (directionY == -1); //If hit something and going down, then set to true
                collisions.above = (directionY == 1); //else set up to true
                collisions.verticalColliderTag = collisionHit.collider.tag;
            }
            if (movementHit) {
                collisions.verticalMovementTag = movementHit.collider.tag;
            }
        }
        if (collisions.climbingSlope) {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x + skinWidth);
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            if (hit) {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle) {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle) {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        if (velocity.y <= climbVelocityY) {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector3 velocity) {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D collisionHit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (collisionHit) {
            float slopeAngle = Vector2.Angle(collisionHit.normal, Vector2.up);
            if(slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
                if(Mathf.Sign(collisionHit.normal.x) == directionX) {
                    if (collisionHit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)) {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void ResetFallingThroughPlatform() {
        collisions.fallingThroughPlatform = false;
    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public float slopeAngle, slopeAngleOld;
        public bool descendingSlope;
        public Vector3 velocityOld;
        public int faceDirection;
        public bool fallingThroughPlatform;
        public string horizontalColliderTag;
        public string verticalColliderTag;
        public string horizontalMovementTag;
        public string verticalMovementTag;

        public void Reset() {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
            horizontalColliderTag = "";
            verticalColliderTag = "";
            horizontalMovementTag = "";
            verticalMovementTag = "";
        }
    }
}
