using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossHUD : MonoBehaviour
{
    public Sprite[] HeartSprites;
    public Image HeartUI;
    public Boss boss;



    void Update()
    {
        HeartUI.sprite = HeartSprites[boss.getBossHealth()];
    }
}
