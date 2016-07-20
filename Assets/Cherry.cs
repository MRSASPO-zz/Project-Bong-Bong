using UnityEngine;
using System.Collections;

public class Cherry : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.SendMessageUpwards("Power");
            Destroy(gameObject);
        }
    }
}
