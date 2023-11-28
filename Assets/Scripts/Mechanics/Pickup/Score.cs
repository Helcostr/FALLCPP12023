using UnityEngine;

public class Score : Pickup {
    [SerializeField]
    int score = 100;
    public override bool effect(PlayerScript player) {
        GameManager.instance.score += score;
        return true;
    }
}
