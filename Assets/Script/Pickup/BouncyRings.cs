using UnityEngine;

public class BouncyRings : CoinPickup {
    private bool ready = false;
    private Rigidbody2D rb;
    protected override void Start() {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (!ready) return;
        if (!collision.collider.CompareTag("Player")) return;
        Destroy(rb);
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
