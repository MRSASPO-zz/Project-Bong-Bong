using UnityEngine;
using System.Collections;

public class NormalLaserManager : MonoBehaviour {
    public LaserBeam[] beams;

    public float cooldownTime = 8;
    public bool isCoolingDown;
    float cooldownTimer;
    // Use this for initialization
    void Start() {
        isCoolingDown = false;
        foreach (LaserBeam beam in beams) {
            beam.activated = true;
            beam.refresh(isCoolingDown);
        }
    }

    // Update is called once per frame
    void Update() {
        StartFiring();
        firingCDTimer();
    }

    public void StartFiring() {
        if (!isCoolingDown) {
            isCoolingDown = true;
            cooldownTimer = cooldownTime;
            foreach (LaserBeam beam in beams) {
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
                foreach (LaserBeam beam in beams) {
                    beam.refresh(isCoolingDown);
                }
            }
        }
    }
}
