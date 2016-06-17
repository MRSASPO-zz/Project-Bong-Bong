using UnityEngine;
using System.Collections;

public class CameraMovementL14 : MonoBehaviour {
    public float speed;
    public float verticalOffset;
    public float verticalSmoothTime;
    public Vector2 focusAreaSize;
    public Controller2D target; //Only used to track the y axis of player

    float smoothVelocityY;
    FocusArea focusArea;

	// Use this for initialization
	void Start () {
        focusArea = new FocusArea(target.collider.bounds, focusAreaSize);
    }

    void LateUpdate() {
        focusArea.Update(target.collider.bounds);

        Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset;

        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
        //focusPosition += Vector2.right * currentLookAheadX;

        Vector3 goalTransformedPosition = (Vector3)focusPosition + Vector3.forward * -10;

        //Change the clamp value if needed
        transform.position = goalTransformedPosition;
    }

    struct FocusArea {
        public Vector2 centre;
        public Vector2 velocity;
        float left, right;
        float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size) {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds) {
            float shiftY = 0;
            if (targetBounds.min.y < bottom) {
                shiftY = targetBounds.min.y - bottom;
            } else if (targetBounds.max.y > top) {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(0, shiftY);//shiftX = 0
        }
    }
}
