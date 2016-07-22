using UnityEngine;
using System.Collections;

public class MissilePlatform : MonoBehaviour {

    public GameObject hybridMissilePU;
    public GameObject hybridMissile;
    private bool firePU = false;
    private bool fire = false;

	// Update is called once per frame
	void Update () {
	    if (fire)
        {
            GameObject missile = (GameObject)Instantiate(hybridMissile, transform.position, Quaternion.identity);
            fire = false;
        }
        else if (firePU)
        {
            GameObject missile = (GameObject)Instantiate(hybridMissilePU, transform.position, Quaternion.identity);
            firePU = false;
        }
	}

    public void Fire()
    {
        fire = true;
    }

    public void FirePU()
    {
        firePU = true;
    }
}
