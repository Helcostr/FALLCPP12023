using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyRings : CoinPickup {
    private bool ready = false;
    protected override void Start() {
        base.Start();
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        OnTriggerEnter2D(collision.collider);
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (!ready) return;
        base.OnTriggerEnter2D(other);
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        ready = true;
        GetComponent<Collider2D>().isTrigger = false;
    }
}
