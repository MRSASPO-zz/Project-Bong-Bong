using UnityEngine;
using System.Collections;

public class MainMenuUI : MonoBehaviour {
    GameObject optionObjects;
    GameObject startObjects;
    // Use this for initialization
    void Start () {
        startObjects = GameObject.FindGameObjectWithTag("ShowOnPause"); //Tag reuse
        optionObjects = GameObject.FindGameObjectWithTag("Options");
        hideOptions();
    }

    public void showOptions() {
        startObjects.SetActive(false);
        optionObjects.SetActive(true);
    }

    public void hideOptions() {
        startObjects.SetActive(true);
        optionObjects.SetActive(false);
        SaveLoad.Save();
    }
}
