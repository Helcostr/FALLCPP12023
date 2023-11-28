using UnityEngine;

public class Invincible : Pickup {
    [SerializeField]
    float duration = 10f; // in seconds btw
    public override bool effect(PlayerScript player) {
        player.triggerInvincible(duration);
        return true;
    }
}
