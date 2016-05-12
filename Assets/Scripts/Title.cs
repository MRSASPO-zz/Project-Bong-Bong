using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Timers;

public class Title : MonoBehaviour {
    public Text title;
    // Use this for initialization
    void Start () {
        Timer delay = new Timer(2000);
        title.text = "Project: ";
        title.text += "Bong ";
        delay.Start();
        delay.Stop();
        title.text += "Bong";
        delay.Start();
        delay.Stop();
    }
	
	// Update is called once per frame
	void Update () {
	    
    }
}
