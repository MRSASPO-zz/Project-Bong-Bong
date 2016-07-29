using UnityEngine;
using System.Collections;

public class PerfExpTeleport : MonoBehaviour {

    private Animator anim;
    public GameObject bossSprite;
    public Boss boss;
    public int threshold;
    private bool hasPlayed = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update() {
        if (boss.getBossHealth()==threshold && !hasPlayed)
        {
            hasPlayed = true;
            anim.Play("Perfect Experiment Teleport");
        }
    }

    public void DestroyGameObject()
    {
        Destroy(bossSprite.gameObject);
        anim.Play("PerfExpTeleportEnd");
    }
}
