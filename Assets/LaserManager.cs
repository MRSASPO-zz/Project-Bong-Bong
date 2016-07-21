using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserManager : MonoBehaviour {
    [Range(0, 1)]
    public int currentPhase;

    public LaserBeam[] phaseZeroBeams;
    public LaserBeam[] phaseOneBeams;
    Dictionary<int, LaserBeam[]> phaseBeamDictionary = new Dictionary<int, LaserBeam[]>();

    public float cooldownTime;
    private bool isCoolingDown;
    public float cooldownTimer;
    
	void Awake () {
        isCoolingDown = false;
        phaseBeamDictionary.Add(0, phaseZeroBeams);
        phaseBeamDictionary.Add(1, phaseOneBeams);
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
                    beam.refresh();
                }
            }
        }
    }
}
