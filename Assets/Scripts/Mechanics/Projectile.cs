using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
    typeof(Rigidbody2D),
    typeof(CircleCollider2D)
)]
public class Projectile : MonoBehaviour {
    public int damage;
    public float lifetime = 10f;
    public int maxBounce = 3;
    [HideInInspector]
    public float speed;
    private int bounceCounter = 0;

    // Start is called before the first frame update
    void Start() {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 1);

        // What I can interact with
        CircleCollider2D collider = GetComponent<CircleCollider2D>();

        // Emergency destroy
        if (lifetime <= 0) lifetime = 10f;
        if (damage <= 0) damage = 2;
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnCollisionEnter2D (Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            onBounce();
        if (other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("PlayerProjectile")) {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    void onBounce() {
        if (++bounceCounter > maxBounce)
            Destroy(gameObject);
    }   
}
