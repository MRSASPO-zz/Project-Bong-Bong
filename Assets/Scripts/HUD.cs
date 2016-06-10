using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public Sprite[] HeartSprites;
    public Image HeartUI;
    private Player player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
       // print(player.GetCurrHealth());
        HeartUI.sprite = HeartSprites[player.GetCurrHealth()];
    }
}
