using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    private int bossMaxHealth = 3;
    private bool invulnerable = false;
    public int bossDamage = 0;
    private SpriteRenderer sr;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

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

    virtual public int getBossHealth()
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
            StartCoroutine(invulnerability());
        }
    }

    IEnumerator invulnerability() {
        invulnerable = true;
        Color32 colorOriginal = sr.color;
        Color32 faded = colorOriginal;
        faded.a /= 4; //reduces the alpha to give it a faded look
        float startInvulTime = Time.time;
        while ((Time.time - startInvulTime) < 1) {
            sr.color = faded;
            yield return new WaitForSeconds(0.1f);
            sr.color = colorOriginal;
            yield return new WaitForSeconds(0.2f);
        }
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