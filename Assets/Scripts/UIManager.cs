using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    GameObject[] pauseObjects;
    GameObject[] playObjects;
    // Use this for initialization
    void Start() {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        playObjects = GameObject.FindGameObjectsWithTag("HideOnPause");
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
        foreach (GameObject g in pauseObjects) {
            g.SetActive(true);
        }
        foreach (GameObject g in playObjects) {
            g.SetActive(false);
        }
    }

    //hides objects with ShowOnPause tag
    public void hidePaused() {
        foreach (GameObject g in pauseObjects) {
            g.SetActive(false);
        }
        foreach (GameObject g in playObjects) {
            g.SetActive(true);
        }
    }

    public void loadLevel(string levelName) {
        Debug.Log("loading this level: " + levelName);
        SceneManager.LoadScene("Scenes/" + levelName);
    }
}
