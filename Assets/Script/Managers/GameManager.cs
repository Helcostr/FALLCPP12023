using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour {
    static GameManager instance = null;
    public static GameManager Instance => instance;

    private int _score = 0;
    public int score {
        get => _score;
        set {
            _score = value;
        }
    }
    private int _lives = 3;
    public int lives {
        get => _lives;
        set {
            if (_lives > value) respawn();

            _lives = value;

            if (_lives > maxLives)
                _lives = maxLives;

            if (_lives < 0) gameOver();

            Debug.Log("Lives has ben set to: " + _lives.ToString());
        }
    }
    public int maxLives = 5;

    public PlayerCtrl playerPrefab;
    [HideInInspector] public PlayerCtrl playerInstance;
    [HideInInspector] public Transform spawnPoint;

    // Start is called before the first frame update
    private void Start() {
        if (instance) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            switch (SceneManager.GetActiveScene().name) {
                case "GameOver":
                    SceneManager.LoadScene("Title");
                    break;
                case "Level":
                    Time.timeScale = (Time.timeScale + 1) % 2;
                    break;
                case "Title":
                    SceneManager.LoadScene("Level");
                    break;
            }
        }
    }

    public void SpawnPlayer(Transform spawnLocation) {
        playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
        spawnPoint = spawnLocation;
    }

    public void respawn() {
        playerInstance.transform.position = spawnPoint.position;
    }
    private void gameOver() {
        SceneManager.LoadScene("GameOver");
    }
    public void startGame() {
        SceneManager.LoadScene("Level");
    }
    public void quitGame() {
        Application.Quit();
    }

    public void UpdateSpawnPoint(Transform updatedPoint) {
        spawnPoint = updatedPoint;
    }
}