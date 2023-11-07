using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
    typeof(Rigidbody2D),
    typeof(SpriteRenderer),
    typeof(Animator)
)]
public class PlayerScript : MonoBehaviour {
    //Component Vars
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    //Movement Vars
    public float speed = 2.0f;
    public float jumpForce = 225.0f;

    //GroundCheck stuff
    public bool isGrounded;
    Transform groundCheck;
    public LayerMask isGroundLayer = 8; // 3rd bit flipped, or 2^3
    public float groundCheckRadius = 0.02f;

    void Start() {
        // Load components
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Check for dirty data
        if (rb == null) Debug.Log("Missing Rigidbody2D");
        if (sr == null) Debug.Log("Missing SpriteRenderer");
        if (anim == null) Debug.Log("Missing Animator");
        if (isGroundLayer == 0) Debug.Log("Not checking any layer as ground. I have no idea what ground is. Help.");
        if (groundCheckRadius <= 0) {
            Debug.Log("Init GroundCheckRadius negative. Setting to positive.");
            groundCheckRadius *= -1;
        }
        if (speed <= 0) {
            Debug.Log("Init Speed negative. Setting to positive.");
            speed *= -1;
        }
        if (jumpForce <= 0) {
            Debug.Log("Init JumpForce negative. Setting to positive.");
            jumpForce *= -1;
        }

        // Forced Settings
        rb.freezeRotation = true;

        // Init groundCheck
        GameObject obj = new GameObject();
        obj.transform.SetParent(gameObject.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.name = "GroundCheck";
        groundCheck = obj.transform;
    }

    // Update is called once per frame
    void Update() {
        float hIn = Input.GetAxis("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        Vector2 moveDirection =
            new Vector2(hIn*speed, rb.velocity.y);
        rb.velocity = moveDirection;

        if (Input.GetButtonDown("Jump") && isGrounded)
            rb.AddForce(Vector2.up * jumpForce);

        // Animations
        anim.SetBool("onGround", isGrounded);
        anim.SetFloat("vVel", rb.velocity.y);
        anim.SetFloat("hIn", Mathf.Abs(hIn));
        anim.SetBool("intentMove", Input.GetAxisRaw("Horizontal") != 0);
        anim.SetBool("isFiring", Input.GetButtonDown("Fire1"));

        anim.SetBool("isDumb", Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D));

        // Flippy birds
        if (hIn > 0)
            sr.flipX = false;
        if (hIn < 0)
            sr.flipX = true;
    }
}
