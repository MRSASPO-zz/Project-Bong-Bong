using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour {
    public AudioClip[] BGMusic;
    private static BGM instance = null;
    private static AudioSource audio;
    public static BGM Instance {
        get { return instance; }
    }

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
            audio = GetComponent<AudioSource>();
            audio.ignoreListenerVolume = true;
            audio.clip = BGMusic[0];
            doLoadLevel();
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private string getLevelString(string sceneName) {
        string stripped = Regex.Replace(sceneName, @"\D", "");
        return stripped;
    }

    private void doLoadLevel() {
        OnLevelWasLoaded(0);
    }

    void OnLevelWasLoaded(int level) {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!(sceneName.Equals("Scenes/Main Menu") 
            || sceneName.Equals("Scenes/Level Select") 
            || sceneName.Equals("Main Menu") 
            || sceneName.Equals("Level Select") 
            || sceneName.Equals("Scenes/Win") 
            || sceneName.Equals("Win"))) {
            string levelString = getLevelString(sceneName);
            int chapterNo = (int)char.GetNumericValue(levelString[0]);
            switch (chapterNo) {
                case 1:
                    if (!audio.clip.Equals(BGMusic[1])) {
                        audio.Stop();
                        audio.clip = BGMusic[1];
                        audio.Play();
                    }
                    break;
                case 2:
                    if (!audio.clip.Equals(BGMusic[2])) {
                        audio.Stop();
                        audio.clip = BGMusic[2];
                        audio.Play();
                    }
                    break;
                case 3:
                    int levelNo = (int)char.GetNumericValue(levelString[1]);
                    if (!(levelNo == 2)) {
                        if (!audio.clip.Equals(BGMusic[3])) {
                            audio.Stop();
                            audio.clip = BGMusic[3];
                            audio.Play();
                        }
                    } else {
                        if (!audio.clip.Equals(BGMusic[4])) {
                            audio.Stop();
                            audio.clip = BGMusic[4];
                            audio.Play();
                        }
                    }
                    break;
            }
        } else {
            if (!audio.clip.Equals(BGMusic[0])) {
                audio.Stop();
                audio.clip = BGMusic[0];
                audio.Play();
            } else {
                if (!audio.isPlaying) {
                    audio.Play();
                }
            }
        }
    }

    public void swapClip(int BGMusicIdx) {
        audio.Stop();
        print("here");
        audio.clip = BGMusic[BGMusicIdx];
        audio.Play();
    }
}
