using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(
    typeof(Animator),
    typeof(SpriteRenderer),
    typeof(Rigidbody2D)
), RequireComponent(
    typeof(Collider2D)
)]
public class PlayerCtrl : MonoBehaviour {
    // Cached Components
    private Animator animator;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    // Speed
    [SerializeField]
    private float baseSpeed = 2.0f;
    private float speedMod = 1f;
    private float speed {
        get => baseSpeed * speedMod;
        set { }
    }
    [SerializeField]
    private float maxSpeed = 50f;
    [SerializeField]
    private float jumpForce = 225.0f;

    //GroundCheck stuff
    private bool isGrounded;
    private bool isJumping;
    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private float groundCheckRadius = 0.02f;

    private Coroutine isInvincible;

    void Start() {
        // Set cache
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        gameObject.layer = LayerMask.NameToLayer("Player");
        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");

        // Validation
    }
    void Update() {
        float hIn = Input.GetAxisRaw("Horizontal");
        bool intentMove = hIn != 0;

        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayerMask);

        rb.AddForce(new Vector2(hIn * speed, 0));
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y, 0);
        /*
        Vector3 vel = rb.velocity;
        rb.AddForce(new Vector2(hIn * speed,0));
        vel.x = Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed);
        rb.velocity = vel;
        */

        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb.AddForce(Vector2.up * jumpForce);
            animator.Play("Jump");
            isJumping = true;
        } else if (rb.velocity.y < 0.01 && isGrounded)
            isJumping = false;

        // Animations
        animator.SetBool("crouch", Input.GetAxisRaw("Vertical") < 0 && rb.velocity == Vector2.zero);
        animator.SetBool("lookUp", Input.GetAxisRaw("Vertical") > 0 && rb.velocity == Vector2.zero);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("onGround", isGrounded);
        animator.SetFloat("hVel", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("vVel", rb.velocity.y);
        animator.SetBool("intentMove", intentMove);
        //animator.SetBool("isFiring", Input.GetButtonDown("Fire1"));

        //animator.SetBool("isDumb", Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D));

        // Flippy birds
        if (hIn > 0)
            sr.flipX = false;
        if (hIn < 0)
            sr.flipX = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            Enemy focus = collision.gameObject.GetComponent<Enemy>();
            if (isInvincible != null) {
                focus.kill();
                return;
            }


            ContactPoint2D test = collision.GetContact(0);
            if (
                test.point.y > collision.gameObject.transform.position.y                
            ) {
                focus.kill();
            } else {
                GameManager.Instance.lives -= 10;
            }
            
            rb.velocity = new Vector2(rb.velocity.x * -1f, 6f);
        }
    }
    public void invincible() {
        if (isInvincible != null)
            StopCoroutine(isInvincible);
        isInvincible = StartCoroutine(_invincible());
    }
    private IEnumerator _invincible() {
        if (isInvincible == null)
            GameManager.Instance.themeMusic();
        yield return new WaitForSeconds(30f);
        isInvincible = null;
        GameManager.Instance.gameMusic();
    }
}
