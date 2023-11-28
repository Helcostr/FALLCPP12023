using UnityEngine;

public class Speed : Pickup {
    [SerializeField]
    float duration = 10f; // in seconds btw
    [SerializeField]
    float speedMod = 2;
    public override bool effect(PlayerScript player) {
        player.triggerSpeedier(speedMod, duration);
        return true;
    }
}
