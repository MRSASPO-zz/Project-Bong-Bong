using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class LaserLever : MonoBehaviour {
    public UnityEvent triggerLasers = new UnityEvent();

    public void PushLever() {
        triggerLasers.Invoke();
    }
}
