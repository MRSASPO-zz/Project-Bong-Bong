using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class snekEat : MonoBehaviour {
    public Vector3 heightToJump;
    public Vector3 targetPosition;
    public Vector3 originalPosition;
    private SpriteRenderer sr;
    public UnityEvent ue;
    public Boss boss;

    // Use this for initialization
    IEnumerator Start() {
        targetPosition = transform.position + heightToJump;
        originalPosition = transform.position;
        sr = GetComponent<SpriteRenderer>();
        yield return StartCoroutine(MoveObject(transform, originalPosition, targetPosition, 3.0f, true));
        yield return StartCoroutine(MoveObject(transform, targetPosition, originalPosition, 3.0f, false));
    }

    //public IEnumerator startMoving() {
        
    //}

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.layer == LayerMask.NameToLayer("Blocking Enemy")) {
            boss.Damage();
            col.gameObject.SetActive(false);
            ue.Invoke();
        }
    }

    IEnumerator MoveObject(Transform thisTrans, Vector3 startPos, Vector3 endPos, float time, bool isUp) {
        sr.flipX = (isUp) ? false : true;
        float i = 0.0f;
        float rate = 3.0f / time;
        while (i < 1.0f) {
            i += Time.deltaTime*rate;
            thisTrans.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }
}
