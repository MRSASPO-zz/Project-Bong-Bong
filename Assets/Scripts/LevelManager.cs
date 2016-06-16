using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
	public void loadLevel(string name){
		Debug.Log ("loading this level: " + name);
		SceneManager.LoadScene("Scenes/"+name);
	}

    public void something() {
        print("something");
    }
}
