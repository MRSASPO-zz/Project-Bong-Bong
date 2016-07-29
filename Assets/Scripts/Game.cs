using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Game{
    public static Game current;
    public List<List<bool>> levelSaveData = new List<List<bool>>();
    [Range(0, 1)]
    public float SFXvolume;
    [Range(0, 1)]
    public float BGMvolume;

    public Game() {
        for(int i = 0; i < 3; i++) {
            List<bool> subList = new List<bool>();
            for(int j = 0; j< 4; j++) {
                if(i==0 && j == 0) {
                    subList.Add(true);
                } else {
                    subList.Add(false);
                }
            }
            levelSaveData.Add(subList);
        }
        SFXvolume = 0.5f;
        BGMvolume = 0.5f;
    }
}
