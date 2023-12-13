using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField]
    private Vector2 min;
    [SerializeField]
    private Vector2 max;
    void Awake() {
        Vector2 _min = Vector2.zero;
        Vector2 _max = Vector2.zero;
        _min.x = Mathf.Min(min.x, max.x);
        _min.y = Mathf.Min(min.y, max.y);
        _max.x = Mathf.Max(min.x, max.x);
        _max.y = Mathf.Max(min.y, max.y);
        min = _min;
        max = _max;
    }
    void FixedUpdate() {
        cameraMatchMove();
    }
    void cameraMatchMove() {
        Vector3 target = getIdealCameraPos();
        target.z = transform.position.z;
        target.x = Mathf.Clamp(target.x, min.x, max.x);
        target.y = Mathf.Clamp(target.y, min.y, max.y);
        transform.position = target;
    }
    Vector3 getIdealCameraPos() {
        Vector3 avg = Vector3.zero;
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
        int length = objects.Length;
        if (length == 0)
            return transform.position;
        if (length == 1)
            return objects[0].transform.position;
        foreach (GameObject obj in  objects)
            avg += obj.transform.position;
        return avg / length;
    }
}
