using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    GameObject pauseObjects;
    GameObject playObjects;
    GameObject optionObjects;
    // Use this for initialization
    void Start() {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectWithTag("ShowOnPause");
        playObjects = GameObject.FindGameObjectWithTag("HideOnPause");
        optionObjects = GameObject.FindGameObjectWithTag("Options");
        hidePaused();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            pauseControl();
        }
    }

    //Reloads the Level
    public void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //controls the pausing of the scene
    public void pauseControl() {
        if (Time.timeScale == 1) {
            Time.timeScale = 0;
            showPaused();
        } else if (Time.timeScale == 0) {
            Time.timeScale = 1;
            hidePaused();
        }
    }

    //shows objects with ShowOnPause tag
    public void showPaused() {
        pauseObjects.SetActive(true);
        playObjects.SetActive(false);
        optionObjects.SetActive(false);
    }

    //hides objects with ShowOnPause tag
    public void hidePaused() {
        pauseObjects.SetActive(false);
        playObjects.SetActive(true);
        optionObjects.SetActive(false);
        SaveLoad.Save();
    }

    public void showOptions() {
        pauseObjects.SetActive(false);
        playObjects.SetActive(false);
        optionObjects.SetActive(true);
    }

    public void hideOptions() {
        pauseObjects.SetActive(true);
        playObjects.SetActive(false);
        optionObjects.SetActive(false);
        SaveLoad.Save();
    }

    public void loadLevel(string levelName) {
        Debug.Log("loading this level: " + levelName);
        SceneManager.LoadScene("Scenes/" + levelName);
    }
}
