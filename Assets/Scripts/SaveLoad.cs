using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour {
    void Awake() {
        createSaveData();
    }

    public void createSaveData() {
        for(int j = 1; j <= 3; j++) {
            if (PlayerPrefs.GetInt("Chapter "+j, -312941) == -312941) {
                bool[] saveDataArray = new bool[4];
                for (int i = 0; i < saveDataArray.Length; i++) {
                    if (i == 0 && j == 1) {
                        saveDataArray[i] = true;
                    } else {
                        saveDataArray[i] = false;
                    }
                }
                PlayerPrefsX.SetBoolArray("Chapter "+j, saveDataArray);
                foreach(bool boolean in saveDataArray) {
                    print(boolean);
                }
                //print("Chapter " + j);
                //print(PlayerPrefsX.GetBoolArray("Chapter " + j)[0]);
            }
        }
    }

    public void CheckAndLoadLevel(string chpNoAndLevelNo) {
        string toSplit = chpNoAndLevelNo;
        string[] words = toSplit.Split(' ');

        int chapterNo = int.Parse(words[0]);
        int levelNo = int.Parse(words[1]);
        print(chapterNo);
        print(levelNo);
        CheckAndLoadLevel(chapterNo, levelNo);
    }

    public void CheckAndLoadLevel(int chapterNo, int levelNo) {
        if(PlayerPrefsX.GetBoolArray("Chapter " + chapterNo)[levelNo - 1]) {
            //LevelSelector.loadLevel("Level ");
            print("Chapter " + chapterNo);
            print(PlayerPrefsX.GetBoolArray("Chapter " + chapterNo)[levelNo - 1]);
            if(chapterNo == 2 && levelNo == 4) {
                loadLevel("Level " + chapterNo + "-" + levelNo+"A");
            }
            loadLevel("Level "+chapterNo+"-"+levelNo);
        }
    }

    private void loadLevel(string levelName) {
        Debug.Log("loading this level: " + levelName);
        SceneManager.LoadScene("Scenes/" + levelName);
    }
}
