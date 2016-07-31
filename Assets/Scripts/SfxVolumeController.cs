using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SfxVolumeController : MonoBehaviour {
    private Slider slider;

    void Awake() {
        slider = GetComponent<Slider>();
        if (Game.current == null) {
            SaveLoad.Load();
            Game.current = SaveLoad.currentGame;
        }
        slider.value = Game.current.SFXvolume;
        AudioListener.volume = slider.value;
        slider.onValueChanged.AddListener(delegate { adjustVolume(); });
    }

    public void adjustVolume() {
        AudioListener.volume = slider.value;
        Game.current.SFXvolume = slider.value;
    }
}
