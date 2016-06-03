using UnityEngine;
using System.Collections;
using CnControls;

public class Game : MonoBehaviour {
    public Character mPlayer;
    bool[] mInputs;
    bool[] mPrevInputs;

	// Use this for initialization
	void Start () {
        mInputs = new bool[(int)KeyInput.Count];
        mPrevInputs = new bool[(int)KeyInput.Count];

        mPlayer.CharacterInit(mInputs, mPrevInputs);
	}
	
	// Update is called once per frame
	void Update () {
        mInputs[(int)KeyInput.GoRight] = (CnInputManager.GetAxisRaw("Horizontal") > 0);
        mInputs[(int)KeyInput.GoLeft] = (CnInputManager.GetAxisRaw("Horizontal") < 0);
        mInputs[(int)KeyInput.GoDown] = CnInputManager.GetButton("Fire1");
        mInputs[(int)KeyInput.Jump] = CnInputManager.GetButton("Jump");
    }

    void FixedUpdate() {
        mPlayer.CharacterUpdate();
    }
}
