using UnityEngine;
using System.Collections;

public class MovingObject: MonoBehaviour{
    public Vector2 mOldPosition;
    public Vector2 mPosition;

    public Vector2 mOldSpeed;
    public Vector2 mSpeed;

    public Vector2 mScale;

    public AABB mAABB;
    public Vector2 mAABBOffset;

    public Transform mTransform;

    public bool mPushedRightWall;
    public bool mPushesRightWall;

    public bool mPushedLeftWall;
    public bool mPushesLeftWall;

    public bool mWasOnGround;
    public bool mOnGround;

    public bool mWasAtCeiling;
    public bool mAtCeiling;

    public void UpdatePhysics() {
        savePreviousFrameData();

        mPosition += mSpeed * Time.deltaTime; //Updating position using current speed

        if(mPosition.y < 0.0f) {
            mPosition.y = 0.0f;
            mOnGround = true;
        } else {
            mOnGround = false;
        }

        mAABB.center = mPosition + mAABBOffset;

        mTransform.position = new Vector3(Mathf.Round(mPosition.x), Mathf.Round(mPosition.y), -1.0f);
        mTransform.localScale = new Vector3(mScale.x, mScale.y, 1.0f);
    }

    private void savePreviousFrameData() {
        mOldPosition = mPosition;
        mOldSpeed = mSpeed;

        mWasOnGround = mOnGround;
        mPushedRightWall = mPushesRightWall;
        mPushedLeftWall = mPushesLeftWall;
        mWasAtCeiling = mAtCeiling;
    }
}
