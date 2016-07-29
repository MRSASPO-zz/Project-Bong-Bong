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

    public void OnTriggerEnter2D(Collider2D hit) {
        if(hit.tag == "Player") {
            anim.Play("Win");
            string levelString = getLevelString(SceneManager.GetActiveScene().name);

            int currentChapterNo = (int)char.GetNumericValue(levelString[0]); //-1 to get the index
            int currentLevelNo = (int)char.GetNumericValue(levelString[1]); // -1 to get the index
            if(SceneManager.GetActiveScene().name.Equals("Scenes/Level 2-4A")) {
                print(currentChapterNo + " " + currentLevelNo);
            } else if (SceneManager.GetActiveScene().name.Equals("Scenes/Level 2-4B")) {
                print(currentChapterNo + " " + currentLevelNo);
            } else {
                if (currentLevelNo + 1 > 4) {
                    currentLevelNo = 1;
                    currentChapterNo += 1;
                } else {
                    currentLevelNo += 1;
                }
            }
            Game.current.levelSaveData[currentChapterNo-1][currentLevelNo-1] = true;
            SaveLoad.Save();
            Invoke("loadLevel", 2f);
        }
    }

    private string getLevelString(string sceneName) {
        string stripped = Regex.Replace(sceneName, @"\D", "");
        return stripped;
    }
}
