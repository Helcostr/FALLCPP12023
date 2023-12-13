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
    private LayerMask groundLayerMask;
    [SerializeField]
    private float groundCheckRadius = 0.02f;

    void Start() {
        // Set cache
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // Validation
        groundLayerMask = LayerMask.NameToLayer("Ground");
    }

    // Update is called once per frame
    void Update() {
        float hIn = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayerMask);

        Vector3 vel = rb.velocity;
        rb.AddForce(new Vector2(hIn * speed,0));
        vel.x = Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed);
        rb.velocity = vel;

        if (Input.GetButtonDown("Jump") && isGrounded)
            rb.AddForce(Vector2.up * jumpForce);
        if (Input.GetAxisRaw("Vertical") < 0 && rb.velocity == Vector2.zero)
            animator.Play("Crouch");
        // Animations
        animator.SetBool("onGround", isGrounded);
        animator.SetFloat("vVel", rb.velocity.y);
        animator.SetFloat("hIn", Mathf.Abs(hIn));
        animator.SetBool("intentMove", Input.GetAxisRaw("Horizontal") != 0);
        //animator.SetBool("isFiring", Input.GetButtonDown("Fire1"));

        //animator.SetBool("isDumb", Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D));

        // Flippy birds
        if (hIn > 0)
            sr.flipX = false;
        if (hIn < 0)
            sr.flipX = true;
    }
}
