using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserManager : MonoBehaviour {
    [Range(0, 1)]
    public int currentPhase;

    public LaserBeam[] phaseZeroBeams;
    public LaserBeam[] phaseOneBeams;
    Dictionary<int, LaserBeam[]> phaseBeamDictionary = new Dictionary<int, LaserBeam[]>();

    public float cooldownTime = 8;
    public bool isCoolingDown;
    float cooldownTimer;
    
	void Awake () {
        isCoolingDown = false;
        currentPhase = 0;
        phaseBeamDictionary.Add(0, phaseZeroBeams);
        phaseBeamDictionary.Add(1, phaseOneBeams);
        foreach (LaserBeam beam in phaseBeamDictionary[0]) {
            beam.activated = true;
            beam.refresh(isCoolingDown);
        }
        foreach (LaserBeam beam in phaseBeamDictionary[1]) {
            beam.activated = false;
            beam.refresh(isCoolingDown);
        }
    }
	
	void Update () {
        firingCDTimer();
	}

    public void StartFiring() {
        if (!isCoolingDown) {
            isCoolingDown = true;
            cooldownTimer = cooldownTime;
            foreach(LaserBeam beam in phaseBeamDictionary[currentPhase]) {
                beam.shootLaser();
            }
        }
    }

    private void firingCDTimer() {
        if (isCoolingDown) {
            if (cooldownTimer > 0) {
                cooldownTimer -= Time.deltaTime;
            } else {
                isCoolingDown = false;
                foreach (LaserBeam beam in phaseBeamDictionary[currentPhase]) {
                    beam.refresh(isCoolingDown);
                }
            }
        }
    }

    public void swapPhase(int phaseNo) {
        foreach (LaserBeam beam in phaseBeamDictionary[currentPhase]) {
            beam.activated = false;
            beam.refresh(isCoolingDown);
        }
        currentPhase = phaseNo % 2;
        foreach (LaserBeam beam in phaseBeamDictionary[currentPhase]) {
            beam.activated = true;
            beam.refresh(isCoolingDown);
        }
    }
}
