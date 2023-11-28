using UnityEngine;

public class Powerup : Pickup {
    [SerializeField]
    public override bool effect(PlayerScript player) {
        Debug.Log("Woah I am stronger now? IDK what i want my powerup to do");
        return true;
    }
}
