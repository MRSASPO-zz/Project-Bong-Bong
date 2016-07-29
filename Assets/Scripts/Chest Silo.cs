using UnityEngine;
using System.Collections;

public class ChestSilo : MonoBehaviour {
    private int shotsFired = 0;
    private int shotThreshold = 10;
    private float fireDelay = 4f;
    private float fireDelayTimer = 0;
    /*
    void Update()
    {
        if (shotsFired < shotThreshold)
        {
            if (determineMissileType())
            {
                missilePlatforms[GetRandomPlatform()].FirePU();
            }
            else
            {
                missilePlatforms[GetRandomPlatform()].Fire();
            }
            shotsFired += 1;
        }
        else if (shotsFired == shotThreshold)
        {
            if (fireDelayTimer <= 0)
            {
                fireDelayTimer = fireDelay;
                resetFire();
            }
            else
            {
                fireDelayTimer -= Time.deltaTime;
            }
        }
    }*/
}
