using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public GameObject player;
    public float minXClamp;
    public float maxXClamp;

    private void Start() {
        if (minXClamp > maxXClamp) {
            Debug.Log("minXClamp is larger, flipping values");
            float temp = minXClamp;
            minXClamp = maxXClamp;
            maxXClamp = temp;
        }
    }
    private void LateUpdate() {
        // Sanity Checker
        if (player == null) return;
        Vector3 cameraPos;
        cameraPos = transform.position;
        cameraPos.x = Mathf.Clamp(player.transform.position.x, minXClamp, maxXClamp);
        transform.position = cameraPos;
    }
}
