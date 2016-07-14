using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    private int bossMaxHealth = 3;
    public int bossDamage = 0;

    public int getBossHealth()
    {
        if (bossDamage >= bossMaxHealth)
        {
            return 0;
        }else
        {
            return bossMaxHealth - bossDamage;
        }
    }


    void OnTriggerEnter2D(Collider2D bomb)
    {
        if (bomb.tag.Equals("Bomb"))
        {
            Destroy(bomb.gameObject);
        }
    }
}