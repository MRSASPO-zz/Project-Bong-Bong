using UnityEngine;
using System.Collections;

public struct AABB {
    public Vector2 center;
    public Vector2 halfSize;

    public AABB(Vector2 center, Vector2 halfSize) {
        this.center = center;
        this.halfSize = halfSize;
    }

    public bool Overlaps(AABB other) {
        if (Mathf.Abs(center.x - other.center.x) > halfSize.x + other.halfSize.x) return false;
        if (Mathf.Abs(center.y - other.center.y) > halfSize.y + other.halfSize.y) return false;
        return true;
    }
}
