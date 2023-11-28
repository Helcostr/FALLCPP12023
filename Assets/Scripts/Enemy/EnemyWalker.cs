using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyWalker : Enemy {
    Rigidbody2D rb;
    public float xSpeed;
    private Coroutine isFlipping;
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        rb = GetComponent<Rigidbody2D>();

        if (xSpeed <= 0)
            xSpeed = 3;
    }

    public override void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            if (isFlipping != null) StopCoroutine(isFlipping);
            Destroy(gameObject, 2);
        }

        if (damage == 9999) {
            anim.SetTrigger("Squish");
            return;
        }

        base.TakeDamage(damage);

        Debug.Log("Enemy Walker took " + damage.ToString() + " damage");
    }

    // Update is called once per frame
    void Update() {
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        if (curPlayingClips[0].clip.name == "Walk")
            rb.velocity = sr.flipX ? new Vector2(-xSpeed, rb.velocity.y) : new Vector2(xSpeed, rb.velocity.y);
        float floor = box.bounds.min.y;
        float left = box.bounds.min.x;
        float right = box.bounds.max.x;
        float radius = (right - left) / 4;
        int layerMask = 1 << LayerMask.NameToLayer("Ground");
        if (!Physics2D.OverlapCircle(new Vector2(sr.flipX ? left: right, floor), radius, layerMask))
            if (isFlipping == null)
                isFlipping = StartCoroutine(flip());
    }
    IEnumerator flip() {
        sr.flipX = !sr.flipX;
        yield return new WaitForSeconds(1);
        isFlipping = null;
    }
}