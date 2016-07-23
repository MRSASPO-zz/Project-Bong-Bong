using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class LaserLever : MonoBehaviour {
    public LaserManager lm;
    private SpriteRenderer spriteRenderer;
    public Sprite[] images;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (!lm.isCoolingDown) {
            spriteRenderer.sprite = (lm.currentPhase == 0) ? images[0] : images[2];
        } else {
            spriteRenderer.sprite = (lm.currentPhase == 0) ? images[1] : images[3];
        }
    }

    public void PushLever() {
        if (!lm.isCoolingDown) {
            lm.StartFiring();
            spriteRenderer.sprite = (lm.currentPhase == 0) ? images[1] : images[3];
        }
    }
}
