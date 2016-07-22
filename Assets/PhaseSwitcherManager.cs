using UnityEngine;
using System.Collections;

public class PhaseSwitcherManager : MonoBehaviour {
    System.Random rnd = new System.Random();

    public LaserManager[] lmArray;

    public float switchTime = 20f;
    bool switchCD;
    float switchTimer;
	// Update is called once per frame
	void Update () {
        switchPhases();
        switchCooldownTimer();
	}

    void switchCooldownTimer() {
        if (switchCD) {
            if(switchTimer > 0) {
                switchTimer -= Time.deltaTime;
            } else {
                switchCD = false;
            }
        }
    }

    void switchPhases() {
        if (switchCD) {
            return;
        }
        switchTimer = switchTime;
        switchCD = true;
        foreach(LaserManager lm in lmArray) {
            int newPhaseNo = rnd.Next(0, 2);
            lm.swapPhase(newPhaseNo);
        }
    }
}
