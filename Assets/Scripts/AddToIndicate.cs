using UnityEngine;
using System.Collections;

public class AddToIndicate : MonoBehaviour {
    
    void Start() {
        GameObject.FindGameObjectWithTag("Target Indicator").GetComponent<CameraTargetIndicator>().addToList(transform);
    }
}
