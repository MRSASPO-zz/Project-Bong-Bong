using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
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
            Invoke("loadLevel", 2f);
        }
    }
}
