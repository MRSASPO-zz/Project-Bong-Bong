using UnityEngine;
using System.Collections;

public class PerfectExperiment : MonoBehaviour {

    public SpriteRenderer sr;
    public Boss boss;

    public void LaserDamage()
    {
        print("Boss has been lazered");
        boss.Damage();
    }
	
	void Awake()
    {
       sr = GetComponent<SpriteRenderer>();
    }
    
}
