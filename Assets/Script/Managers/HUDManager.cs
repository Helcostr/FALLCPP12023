using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour {
    [SerializeField]
    private TMP_Text ringText;
    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private TMP_Text scoreText;

    // Update is called once per frame
    void Update() {
        if (ringText != null)
            ringText.text = GameManager.Instance.lives == 0 ? "<sprite=10>" : toSpriteText(GameManager.Instance.lives);
        if (timeText != null)
            timeText.text = toSpriteText((int) Time.timeSinceLevelLoad);
        if (scoreText != null)
            scoreText.text = toSpriteText(GameManager.Instance.score);
    }
    string toSpriteText(int number) {
        string output = number.ToString();
        return string.Join("", output.ToCharArray(0, output.Length).Select(e => "<sprite=" + e + ">"));
    }
}
