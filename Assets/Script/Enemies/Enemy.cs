using UnityEngine;
[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(Collider2D))]
public class Enemy : MonoBehaviour {
    protected SpriteRenderer sr;
    protected Animator anim;
    
    protected Collider2D col;
    protected bool isDead = false;
    [SerializeField]
    private AudioClip death;
    protected virtual void Start() {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public virtual void kill() {
        if (isDead) return;
        anim.Play("Death");
        GameManager.Instance.score += 100;
        isDead = true;
        col.isTrigger = true;
        col.enabled = false;
        GameManager.Instance.playSound(death);
    }
    private void destroy() {
        Destroy(gameObject);
    }
}