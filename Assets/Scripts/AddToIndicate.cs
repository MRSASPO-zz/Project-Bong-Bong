using UnityEngine;
using System.Collections;

public class AddToIndicate : MonoBehaviour {
    CameraTargetIndicator camT;
    
    void Start() {
        camT = GameObject.FindGameObjectWithTag("Target Indicator").GetComponent<CameraTargetIndicator>();
        camT.addToList(transform);
    }
}
