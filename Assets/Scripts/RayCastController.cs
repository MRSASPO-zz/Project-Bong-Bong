using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class RayCastController : MonoBehaviour {
    public LayerMask collisionMask;

    public const float skinWidth = .015f;
    public int horizontalRayCount;
    public int verticalRayCount;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;

    [HideInInspector]
    public new BoxCollider2D collider;
    public RaycastOrigins raycastOrigins;

    public virtual void Awake() {
        collider = GetComponent<BoxCollider2D>();
    }

    public virtual void Start() {
        calculateRaySpacing();
    }

    public void UpdateRaycastOrigin() {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

        raycastOrigins.centerLeft = new Vector2(bounds.min.x, bounds.center.y);
        raycastOrigins.centerRight = new Vector2(bounds.max.x, bounds.center.y);
    }

    public void calculateRaySpacing() {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
        public Vector2 centerLeft, centerRight;
    }
}
