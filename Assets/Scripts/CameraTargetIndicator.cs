using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraTargetIndicator : MonoBehaviour {
    public GameObject arrowPrefab;
    public float reduxScale = 0.9f;
    public Camera camera;

    List<GameObject> arrowPool = new List<GameObject>();
    int arrowPoolCursor = 0;
    List<Transform> trackedObjects = new List<Transform>();

    public void addToList(Transform t) {
        trackedObjects.Add(t);
    }

    public void removeFromList(Transform t) {
        trackedObjects.Remove(t);
    }

    void LateUpdate() {
        resetPool();

        Vector3 camOrigin = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 1)).origin; //Origin of the camera
        Bounds camBounds = CameraExtensions.OrthographicBounds(camera);
        float height = camBounds.max.y - camBounds.min.y;
        float width = camBounds.max.x - camBounds.min.x;

        foreach(Transform obj in trackedObjects) {
            Vector3 target = obj.position;
            //Check point is not in camera view
            bool isInRectangle = (Mathf.Abs(target.x - camOrigin.x) - 0.5f) <= width / 2 && (Mathf.Abs(target.y - camOrigin.y) - 0.5f) <= height / 2;

            Vector3 screenPos = camOrigin;
            if (!isInRectangle) {
                Vector3 screenPosNormalised = getScreenPosNormalised(target, camOrigin, height, width, reduxScale);
                screenPos = screenPosNormalised + camOrigin;
                float angle = Mathf.Atan2(screenPosNormalised.y, screenPosNormalised.x) * 180 / Mathf.PI;
                GameObject arrow = getArrow();
                arrow.GetComponent<SpriteRenderer>().color = Color.red;//can reassign later
                arrow.transform.localPosition = screenPos;
                arrow.transform.localRotation = Quaternion.Euler(0, 0, angle);
            }
            Debug.DrawLine(camOrigin, screenPos, Color.yellow);
        }
        
        cleanPool();
    }

    //gets the screenPos of the indicator relative to WORLD SPACE (in (x,y,z), not pixels)
    //reduxScale denotes the distance away from the screen, 1 will be touching the screen while 0 will be at the centerpoint
    Vector3 getScreenPosNormalised(Vector3 target, Vector3 camOrigin, float height, float width, float reduxScale) {
        Vector3 screenPosNormalised = Vector3.zero;
        Vector3 targetNormalised = target - camOrigin; //Normalised to (0, 0)
        float heightRedux = height * reduxScale;
        float widthRedux = width * reduxScale;
        if (targetNormalised.x == 0) {
            if (targetNormalised.y > 0) {
                screenPosNormalised = new Vector3(0, heightRedux / 2, 0);
            } else if (targetNormalised.y < 0) {
                screenPosNormalised = new Vector3(0, -heightRedux / 2, 0);
            }
        } else {
            //y = mx + c, c==0 because normalised
            float m = targetNormalised.y / targetNormalised.x;
            if (targetNormalised.y > 0) {
                //y = mx ==> x = y/m, y will be the top edge, hence y = heightRedux/2
                float topScreenX = heightRedux / (2 * m) ;
                if (Mathf.Abs(topScreenX) >= widthRedux / 2) {
                    //finding for left and right of the screen
                    //y = mx, x = widthRedux/2, but first check if the target.x > 0
                    if (targetNormalised.x > 0) {
                        float rightScreenY = widthRedux / 2 * m ;
                        screenPosNormalised = new Vector3(widthRedux / 2 , rightScreenY, 0);
                    } else {
                        float leftScreenY = -widthRedux / 2 * m ;
                        screenPosNormalised = new Vector3(-widthRedux / 2 , leftScreenY, 0);
                    }
                } else {
                    screenPosNormalised = new Vector3(topScreenX, heightRedux / 2 , 0);
                }
            } else {
                //y = mx ==> x = y/m, y will be the bottom edge, hence y = -heightRedux/2
                float btmScreenX = -heightRedux / (2 * m); //Position of X coordinate along the top edge
                if (Mathf.Abs(btmScreenX) >= widthRedux / 2) {
                    //y = mx, x = widthRedux/2, but first check if the target.x > 0
                    if (targetNormalised.x > 0) {
                        float rightScreenY = widthRedux / 2 * m ;
                        screenPosNormalised = new Vector3(widthRedux / 2 , rightScreenY, 0);
                    } else {
                        float leftScreenY = -widthRedux / 2 * m ;
                        screenPosNormalised = new Vector3(-widthRedux / 2 , leftScreenY, 0);
                    }
                } else {
                    screenPosNormalised = new Vector3(btmScreenX, -heightRedux / 2, 0);
                }
            }
        }
        return screenPosNormalised;
    }

    void resetPool() {
        arrowPoolCursor = 0;
    }

    GameObject getArrow() {
        GameObject output;

        if(arrowPoolCursor < arrowPool.Count) {
            output = arrowPool[arrowPoolCursor]; //reuse existing
        } else {
            output = Instantiate(arrowPrefab) as GameObject;
            output.transform.parent = transform;
            arrowPool.Add(output);
        }
        arrowPoolCursor++;
        return output;
    }

    void cleanPool() {
        while(arrowPool.Count > arrowPoolCursor) {
            GameObject obj2 = arrowPool[arrowPool.Count - 1]; //last
            arrowPool.Remove(obj2);
            Destroy(obj2.gameObject);
        }
    }
}
