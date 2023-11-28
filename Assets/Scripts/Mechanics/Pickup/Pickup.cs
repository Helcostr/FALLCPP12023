using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Pickup : MonoBehaviour {
    public virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerScript temp = other.gameObject.GetComponent<PlayerScript>();
            if (temp == null) return;
            if (effect(temp))
                Destroy(gameObject, 0);
        }
    }
    public abstract bool effect(PlayerScript player);
}
