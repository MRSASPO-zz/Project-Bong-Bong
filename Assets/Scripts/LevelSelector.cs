using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {
    public void loadLevel(string levelName) {
        Debug.Log("loading this level: " + levelName);
        SceneManager.LoadScene("Scenes/" + levelName);
    }
}
