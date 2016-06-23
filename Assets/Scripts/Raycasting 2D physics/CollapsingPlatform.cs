using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class CollapsingPlatform : RayCastController {
    public float timeToFall;
    bool isCollapsed;
    Animator anim;

    //Ideally this platform should not move, but we still put in the update rayorigin inside update
    public override void Start() {
        base.Start();
        UpdateRaycastOrigin();
        this.gameObject.layer = LayerMask.NameToLayer("Obstacle");
        isCollapsed = false;
        anim = GetComponent<Animator>();
    }

    void Update() {
        UpdateRaycastOrigin();
        //Detecting only collision hits from the top, so raycasts go upwards only
        if (!isCollapsed) {
            VerticalCollisions();
        } else {
            if (this.gameObject.layer != LayerMask.NameToLayer("Default")) {
                anim.Play("Collapsing");
                Invoke("FallThroughPlatform", timeToFall);
            } else {
                Destroy(gameObject);
            }
        }
    }

    void FallThroughPlatform() {
        this.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private void VerticalCollisions() {
        float rayLength = skinWidth;

        for (int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D collisionHit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, collisionMask);

            if (collisionHit && collisionHit.collider.tag == "Player") {
                isCollapsed = true;
            }
        }
    }
}
