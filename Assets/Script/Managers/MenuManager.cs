using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Button stopButton;

    void Start() {
        if (playButton)
            playButton.onClick.AddListener(() => GameManager.Instance.startGame());
        if (stopButton)
            stopButton.onClick.AddListener(() => GameManager.Instance.quitGame());
    }
}
