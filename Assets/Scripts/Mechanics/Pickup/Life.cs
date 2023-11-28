using UnityEngine;

public class Life : Pickup {
    [SerializeField]
    public int healthIncrease = 1;
    
    public override bool effect(PlayerScript player) {
        int oldHealth = player.health;
        player.health += healthIncrease;
        return oldHealth != player.health;
    }
}
