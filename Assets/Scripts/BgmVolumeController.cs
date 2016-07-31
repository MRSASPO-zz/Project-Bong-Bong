using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BgmVolumeController : MonoBehaviour {
    private Slider slider;
    private GameObject bgmGO;

	void Awake () {
        slider = GetComponent<Slider>();
        if(Game.current == null) {
            SaveLoad.Load();
            Game.current = SaveLoad.currentGame;
        }
        slider.value = Game.current.BGMvolume;
        bgmGO = GameObject.FindGameObjectWithTag("BGM");
        bgmGO.GetComponent<AudioSource>().volume = slider.value;
        slider.onValueChanged.AddListener(delegate { adjustVolume(); });
    }

	public void adjustVolume() {
        bgmGO.GetComponent<AudioSource>().volume = slider.value;
        Game.current.BGMvolume = slider.value;
    }
}
