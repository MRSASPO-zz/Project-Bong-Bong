using UnityEngine;
using System.Collections;

public class MissilePlatformManager : MonoBehaviour {

    public MissilePlatform[] missilePlatforms;
    private int shotsFired = 0;
    private int shotThreshold = 5;
    private float fireDelay = 4f;
    private float fireDelayTimer = 0;

    // Update is called once per frame
    void Update () {
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
        else if(shotsFired == shotThreshold)
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
	}

    private int GetRandomPlatform()
    {
        int randomNumber = Random.Range(0,31);
        return randomNumber;
    }

    private bool determineMissileType()
    {
        int randomNumber = Random.Range(0, 40);
        return randomNumber%7==0;
    }

    private void resetFire()
    {
        shotsFired = 0;
    }
}
