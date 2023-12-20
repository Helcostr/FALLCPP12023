using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour {
    [SerializeField]
    private AudioClip ghz_intro;
    [SerializeField]
    private AudioClip ghz_loop;
    [SerializeField]
    private AudioClip theme_intro;
    [SerializeField]
    private AudioClip theme_loop;
    [SerializeField]
    private AudioClip gameOverMusic;
    [SerializeField]
    private AudioClip youWinMusic;
    [SerializeField]
    private AudioMixerGroup mixGroupMusic;
    [SerializeField]
    private AudioMixerGroup mixGroupSFX;
    private AudioSource musicPlayer;
    List<AudioSource> soundFX = new List<AudioSource>();
    static GameManager instance = null;
    public static GameManager Instance => instance;

    private int _score = 0;
    public int score {
        get => _score;
        set {
            _score = value;
        }
    }
    private int _lives = 0;
    public int lives {
        get => _lives;
        set {
            if (_lives > value) {
                int temp = _lives - System.Math.Max(value, 0);
                if (value < 0 && temp == 0) gameOver();
                spawnBouncyRings(temp);
            }

            _lives = System.Math.Max(value, 0);

            //if (_lives > maxLives)
            //    _lives = maxLives;

            Debug.Log("Lives has ben set to: " + _lives.ToString());
        }
    }
    public int maxLives = 5;
    [SerializeField]
    private PlayerCtrl playerPrefab;
    [SerializeField]
    private Rigidbody2D bouncyCoinPrefab;
    [SerializeField]
    private AudioClip coinLossSFX;
    [HideInInspector] public PlayerCtrl playerInstance;
    [HideInInspector] public Transform spawnPoint;
    private AudioLooper looper;

    // Start is called before the first frame update
    private void Start() {
        if (instance) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        musicPlayer = gameObject.AddComponent<AudioSource>();
        musicPlayer.outputAudioMixerGroup = mixGroupMusic;

        SceneManager.activeSceneChanged += sceneChanged;
        sceneMusic(SceneManager.GetActiveScene().name);
    }
    private void spawnBouncyRings(int amt) {
        if (amt <= 0) return;
        playSound(coinLossSFX);
        for (int i = 0; i < amt; i++) {
            Rigidbody2D focus = Instantiate(bouncyCoinPrefab,playerInstance.transform.position,Quaternion.identity);
            focus.velocity = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(0f, 1f)
            ).normalized * 5f;
            Destroy(focus.gameObject, 10f);
        }
    }

    private string prevSceneName = "";
    private void sceneChanged(Scene current, Scene next) {
        sceneMusic(next.name);
        prevSceneName = next.name;
    }
    private void sceneMusic(string name) {
        switch (name) {
            case "Level":
                gameMusic();
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Coins"), LayerMask.NameToLayer("Coins"), true);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Coins"), LayerMask.NameToLayer("Enemy"), true);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
                break;
            case "Title":
                themeMusic();
                break;
            case "GameOver":
                musicPlayer.loop = false;
                musicPlayer.clip = gameOverMusic;
                musicPlayer.Play();
                break;
            case "YouWin":
                musicPlayer.loop = false;
                musicPlayer.clip = youWinMusic;
                musicPlayer.Play();
                break;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            switch (SceneManager.GetActiveScene().name) {
                case "GameOver":
                case "YouWin":
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
    public void gameMusic() {
        looper = new AudioLooper(ghz_intro, ghz_loop);
        looper.initPlayer(musicPlayer);
    }
    public void themeMusic() {
        looper = new AudioLooper(theme_intro, theme_loop);
        looper.initPlayer(musicPlayer);
    }

    public void UpdateSpawnPoint(Transform updatedPoint) {
        spawnPoint = updatedPoint;
    }
    public void playSound(AudioClip clip) {
        AudioSource master = null;
        foreach (AudioSource source in soundFX) {
            if (source.isPlaying) continue;

            master = source;
            break;
        }
        if (master == null) {
            master = gameObject.AddComponent<AudioSource>();
            soundFX.Add(master);
            master.outputAudioMixerGroup = mixGroupSFX;
            master.playOnAwake = false;
        }
        master.clip = clip;
        master.Play();
    }
}