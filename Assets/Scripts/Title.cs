using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Timers;

public class Title : MonoBehaviour {
    public Text title;
    // Use this for initialization
    void Start ()
    {
        StartCoroutine(intro());
    }
	
	// Update is called once per frame
	void Update () {
	    
    }

    private IEnumerator intro ()
    {
        title.text = "Project: ";
        yield return new WaitForSeconds(1.5f);
        title.text += "Bong ";
        yield return new WaitForSeconds(1.5f);
        title.text += "Bong";
    }
}
