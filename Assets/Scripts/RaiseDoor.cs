using UnityEngine;
using System.Collections;

public class RaiseDoor : MonoBehaviour {

    public Boss boss;
    public int raiseThreshold;
    public int ycoord;
	
	// Update is called once per frame
	void Update () {
	    if (boss.getBossHealth() == raiseThreshold)
        {
            if (transform.position.y != ycoord)
            {
                transform.Translate(0, 0.01f, 0);
            }
        }
	}
}
