using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class LevelSelector : MonoBehaviour {
    public Text text;
    public Color colour;
    private GameObject[] buttonGOs;

    void Awake() {
        SaveLoad.Load();
        Game.current = SaveLoad.currentGame;
        buttonGOs = GameObject.FindGameObjectsWithTag("LevelButton");
        colour = buttonGOs[0].GetComponent<Image>().color;
        foreach(GameObject buttonGO in buttonGOs) {
            string levelString = getLevelString(buttonGO.name);
            if(Game.current.levelSaveData[(int)char.GetNumericValue(levelString[0]) - 1][(int)char.GetNumericValue(levelString[1]) - 1]) {
                buttonGO.GetComponent<Image>().color = colour;
            } else {
                buttonGO.GetComponent<Image>().color = Color.red;
            }
        }
    }

    private string getLevelString(string sceneName) {
        string stripped = Regex.Replace(sceneName, @"\D", "");
        return stripped;
    }

    private void loadLevel(string levelName) {
        Debug.Log("loading this level: " + levelName);
        SceneManager.LoadScene("Scenes/" + levelName);
    }

    public void CheckAndLoadLevel(string chpNoAndLevelNo) {
        string toSplit = chpNoAndLevelNo;
        string[] words = toSplit.Split(' ');

        int chapterNo = int.Parse(words[0]);
        int levelNo = int.Parse(words[1]);
        CheckAndLoadLevel(chapterNo, levelNo);
    }

    public void CheckAndLoadLevel(int chapterNo, int levelNo) {
        //if (PlayerPrefsX.GetBoolArray("Chapter " + chapterNo)[levelNo - 1]) {
        if (Game.current.levelSaveData[chapterNo - 1][levelNo - 1]) { 
            if (chapterNo == 2 && levelNo == 4) {
                loadLevel("Level " + chapterNo + "-" + levelNo+"A");
            } else {
                loadLevel("Level " + chapterNo + "-" + levelNo);
            }
        } else {
            text.CrossFadeAlpha(1, 0, false);
            text.text = "You have not unlocked Level " + chapterNo + "-" + levelNo + " yet";
            text.CrossFadeAlpha(0, 3, false);
        }
    }
}
