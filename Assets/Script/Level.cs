using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    public int startingLives = 3;
    public Transform spawnPoint;
    void Start() {
        GameManager.Instance.SpawnPlayer(spawnPoint);
        GameManager.Instance.lives = startingLives;
    }
}
