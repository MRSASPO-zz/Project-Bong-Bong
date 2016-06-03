using UnityEngine;
using System.Collections;

public class Character : MovingObject {
    protected bool[] mInputs;
    protected bool[] mPrevInputs;

    public CharacterState mCurrentState = CharacterState.Stand;
    public float mJumpSpeed;
    public float mWalkSpeed;

    public void CharacterInit(bool[] inputs, bool[] prevInputs) {
        mPosition = transform.position;
        mAABB.halfSize = new Vector2(Constants.cHalfSizeX, Constants.cHalfSizeY);
        mAABBOffset.y = mAABB.halfSize.y;

        mInputs = inputs;
        mPrevInputs = prevInputs;

        mJumpSpeed = Constants.cJumpSpeed;
        mWalkSpeed = Constants.cWalkSpeed;

        mScale = Vector2.one;
    }

	public void CharacterUpdate() {
        switch (mCurrentState) {

            case CharacterState.Stand:
                mSpeed = Vector2.zero;
                //mAnimator.Play("Stand"); //This is to set the state of the Animator
                //If Character is not on the ground, then set the state to Jump
                if (!mOnGround) {
                    mCurrentState = CharacterState.Jump;
                    break;
                }
                //If Character Moves left or right, then set to walk, else if Input is Jump then set to jump
                if(KeyState(KeyInput.GoRight) != KeyState(KeyInput.GoLeft)) {
                    mCurrentState = CharacterState.Walk;
                    break;
                } else if (KeyState(KeyInput.Jump)) {
                    mSpeed.y = mJumpSpeed;
                    mCurrentState = CharacterState.Jump;
                    break;
                }
                break;

            case CharacterState.Walk:
                //mAnimator.Play("Stand"); //This is to set the state of the Animator
                /*If neither Right nor Left are pressed, or both are pressed together, 
                then set state to Stand. else if Right is pressed, else if left is pressed
                */
                if(KeyState(KeyInput.GoRight) == KeyState(KeyInput.GoLeft)) {
                    mCurrentState = CharacterState.Stand;
                    mSpeed = Vector2.zero;
                    break;
                } else if (KeyState(KeyInput.GoRight)) {
                    if (mPushesRightWall) {
                        mSpeed.x = 0.0f;
                    } else {
                        mSpeed.x = mWalkSpeed;
                    }
                    mScale.x = Mathf.Abs(mScale.x); //scale to change horizontal axes
                } else if (KeyState(KeyInput.GoLeft)) {
                    if (mPushesLeftWall) {
                        mSpeed.x = 0.0f;
                    }else {
                        mSpeed.x = -mWalkSpeed;
                    }
                    mScale.x = -Mathf.Abs(mScale.x); //scale
                }

                if (KeyState(KeyInput.Jump)) {
                    mSpeed.y = mJumpSpeed;
                    //mAudioSource.PlayOneShot(mJumpSfx, 1, 0f); //Assuming if there is audio
                    mCurrentState = CharacterState.Jump;
                    break;
                } else if (!mOnGround) {
                    mCurrentState = CharacterState.Jump;
                    break;
                }
                break;

            case CharacterState.Jump:
                //mAnimator.Play("Jump"); //Jump Animations
                mSpeed.y += Constants.cGravity * Time.deltaTime;
                mSpeed.y = Mathf.Max(mSpeed.y, Constants.cMaxFallingSpeed);

                if (KeyState(KeyInput.GoRight) == KeyState(KeyInput.GoLeft)) {
                    mSpeed.x = 0.0f;
                } else if (KeyState(KeyInput.GoRight)) {
                    if (mPushesRightWall) {
                        mSpeed.x = 0.0f;
                    } else {
                        mSpeed.x = mWalkSpeed;
                    }
                    mScale.x = Mathf.Abs(mScale.x); //scale to change horizontal axes
                } else if (KeyState(KeyInput.GoLeft)) {
                    if (mPushesLeftWall) {
                        mSpeed.x = 0.0f;
                    } else {
                        mSpeed.x = -mWalkSpeed;
                    }
                    mScale.x = -Mathf.Abs(mScale.x); //scale
                }

                if(!KeyState(KeyInput.Jump) && mSpeed.y > 0.0f) {
                    mSpeed.y = Mathf.Min(mSpeed.y, Constants.cMinJumpSpeed);
                }
                break;

            case CharacterState.GrabLedge:
                break;
        }

        UpdatePhysics();
        //Assume that there is audio
        /*
        if ((mOnGround && !mWasOnGround)
        || (!mWasAtCeiling && mAtCeiling)
        || (!mPushedLeftWall && mPushesLeftWall)
        || (!mPushedRightWall && mPushesRightWall)){
            mAudioSource.PlayOneShot(mHitWallSfx, 0.5f); 
        }
        */
        UpdatePrevInputs();
    }

    public void UpdatePrevInputs() {
        var count = (byte)KeyInput.Count;
        for(byte i=0; i< count; ++i) {
            mPrevInputs[i] = mInputs[i];
        }
    }

    protected bool Released(KeyInput key) {
        return (!mInputs[(int)key] && mPrevInputs[(int)key]);
    }

    protected bool KeyState(KeyInput key) {
        return (mInputs[(int)key]);
    }

    protected bool Pressed(KeyInput key) {
        return (mInputs[(int)key] && !mPrevInputs[(int)key]);
    }
}
