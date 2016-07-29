using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad {
    public static Game currentGame = null;
    public static void Save() {
        currentGame = Game.current;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, currentGame);
        file.Close();
    }

    public static void Load() {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveLoad.currentGame = (Game)bf.Deserialize(file);
            file.Close();
        } else {
            currentGame = new Game();
            Game.current = currentGame;
            Save();
            Load();
        }
    }
}
