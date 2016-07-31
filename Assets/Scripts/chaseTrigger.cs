using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class chaseTrigger : MonoBehaviour {
    public UnityEvent ue = new UnityEvent();

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            ue.Invoke();
        }
    }
}
