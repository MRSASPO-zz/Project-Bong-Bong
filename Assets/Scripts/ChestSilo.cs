using UnityEngine;
using System.Collections;

public class ChestSilo : MonoBehaviour
{

    public GameObject bullet;
    private int shotsFired = 0;
    private int shotsThreshold = 2;
    private float fireDelay = 4f;
    private float fireDelayTimer = 4f;
    private float shotDelay = 0.5f;
    private float shotDelayTimer = 0.5f;
    private bool isTriggered = false;


    void Update()
    {
        if (isTriggered)
        {
            if (shotsFired < shotsThreshold)
            {
                if (shotDelayTimer <= 0)
                {
                    GameObject missile = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
                    shotDelayTimer = shotDelay;
                    shotsFired += 1;
                }
                else
                {
                    shotDelayTimer -= Time.deltaTime;
                }
            }
            else if (shotsFired == shotsThreshold)
            {
                if (fireDelayTimer <= 0)
                {
                    fireDelayTimer = fireDelay;
                    shotsFired = 0;
                }
                else
                {
                    fireDelayTimer -= Time.deltaTime;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isTriggered = true;
        }
    }
}
