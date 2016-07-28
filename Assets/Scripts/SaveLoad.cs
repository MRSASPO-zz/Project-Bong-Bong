using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveLoad {
    public void createSaveData() {
        for(int j = 1; j <= 3; j++) {
            if (!PlayerPrefs.HasKey("Chapter "+j)) {
                bool[] saveDataArray = new bool[4];
                for (int i = 0; i < saveDataArray.Length; i++) {
                    if (i == 0 && j == 1) {
                        saveDataArray[i] = true;
                    }
                    saveDataArray[i] = false;
                }
                PlayerPrefsX.SetBoolArray("Chapter "+j, saveDataArray);
            }
        }
    }

    public void CheckAndLoadLevel(int chapterNo, int levelNo) {
        if(PlayerPrefsX.GetBoolArray("Chapter " + chapterNo)[levelNo - 1]) {
            LevelSelector.loadLevel("Level ");
        }
    }
}
