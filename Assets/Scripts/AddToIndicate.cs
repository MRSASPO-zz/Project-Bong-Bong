using UnityEngine;
using System.Collections;

public class AddToIndicate : MonoBehaviour {
    CameraTargetIndicator cTI;
    void Start() {
        cTI = GameObject.FindGameObjectWithTag("Target Indicator").GetComponent<CameraTargetIndicator>();
        cTI.addToList(transform);
    }

    public void removeFromList() {
        cTI.removeFromList(transform);
    }
}
