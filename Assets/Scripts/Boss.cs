using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    private int bossMaxHealth = 3;
    private bool invulnerable = false;
    public int bossDamage = 0;

    void Update()
    {
        if (bossIsDead())
        {
            Destroy(gameObject);
        }
    }

    private bool bossIsDead()
    {
        return getBossHealth()==0;
    }

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


    void OnTriggerEnter2D(Collider2D contact)
    {
        if (contact.tag.Equals("Lethal"))
        {
            TakeLethalDamage();
        }
    }

    private int GetBossMaxHealth()
    {
        return bossMaxHealth;
    }

    public void Damage()
    {
        if (!invulnerable)
        {
            TakeDamage();
            invulnerability();
            Invoke("resetInvulnerability", 1.0f);
        }
    }

    public void invulnerability()
    {
        invulnerable = true;
    }

    public void resetInvulnerability()
    {
        invulnerable = false;
    }

    public void TakeDamage()
    {
        bossDamage += 1;
    }

    public void TakeLethalDamage()
    {
        bossDamage = bossMaxHealth;
    }
}