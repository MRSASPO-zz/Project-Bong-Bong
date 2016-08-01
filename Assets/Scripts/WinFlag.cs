using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class WinFlag : MonoBehaviour {
    public string levelName;
    Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
    }

	public void loadLevel(){
		Debug.Log ("loading this level: " + levelName);
		SceneManager.LoadScene("Scenes/"+levelName);
	}

    private bool excludedLevels() {
        string sceneName = SceneManager.GetActiveScene().name;
        return sceneName.Equals("Scenes/Main Menu") ||
            sceneName.Equals("Scenes/Level Select") ||
            sceneName.Equals("Main Menu") ||
            sceneName.Equals("Level Select") ||
            sceneName.Equals("Scenes/Win") ||
            sceneName.Equals("Win") ||
            sceneName.Equals("Scenes/Level 2-4A") ||
            sceneName.Equals("Level 2-4A") ||
            sceneName.Equals("Scenes/Level 2-4B") ||
            sceneName.Equals("Level 2-4B") ||
            sceneName.Equals("Scenes/Level 3-4") ||
            sceneName.Equals("Level 3-4");
    }

    public void OnTriggerEnter2D(Collider2D hit) {
        if(hit.tag == "Player") {
            anim.Play("Win");
            if(!excludedLevels()) {
                string levelString = getLevelString(SceneManager.GetActiveScene().name);
                int currentChapterNo = (int)char.GetNumericValue(levelString[0]); //-1 to get the index
                int currentLevelNo = (int)char.GetNumericValue(levelString[1]); // -1 to get the index
                if (currentLevelNo + 1 > 4) {
                    currentLevelNo = 1;
                    currentChapterNo += 1;
                } else {
                    currentLevelNo += 1;
                }
                //print("currentChpNo and levelNo" + currentChapterNo + " " + currentLevelNo);
                Game.current.levelSaveData[currentChapterNo - 1][currentLevelNo - 1] = true;
                SaveLoad.Save();
            }
            Invoke("loadLevel", 2f);
        }
    }

    private string getLevelString(string sceneName) {
        string stripped = Regex.Replace(sceneName, @"\D", "");
        return stripped;
    }
}
