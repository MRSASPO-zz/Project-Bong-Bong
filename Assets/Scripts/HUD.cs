using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public Sprite[] HeartSprites;
    public Image HeartUI;
    public Player player;



    void Update()
    {
        //print(player.GetCurrHealth());
        HeartUI.sprite = HeartSprites[player.GetCurrHealth()];
    }
}
