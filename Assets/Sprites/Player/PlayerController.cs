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

    //Movement Vars
    public float speed = 2.0f;
    public float jumpForce = 225.0f;

    //GroundCheck stuff
    Transform groundCheck;
    public LayerMask isGroundLayer = 8;
    public float groundCheckRadius = 0.02f;

    void Start() {
        // Load components
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // Check for dirty data
        if (rb == null) Debug.Log("Missing Rigidbody2D");
        if (sr == null) Debug.Log("Missing SpriteRenderer");
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
    public bool isGrounded {
        get {
            return Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                isGroundLayer);
        }
    }

    // Update is called once per frame
    void Update() {
        float hIn = Input.GetAxis("Horizontal");

        Vector2 moveDirection =
            new Vector2(hIn*speed,rb.velocity.y);
        rb.velocity = moveDirection;

        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb.AddForce(Vector2.up * jumpForce);
        }
    }
}
