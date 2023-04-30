using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 14f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.5f;
    public Vector3 groundCheckOffset; // Add this line
    private Animator animator;
    public Transform respawnPoint;


    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isRun;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (moveInput != 0) {
            animator.SetBool("isRun", true);
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        } else 
        {
            animator.SetBool("isRun", false);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position + groundCheckOffset, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null && hit.collider.gameObject != null;

        Debug.DrawRay(transform.position + groundCheckOffset, Vector2.down * groundCheckDistance, Color.red);

        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            animator.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        } else
        {
            animator.SetBool("isJumping", !isGrounded);
        }

        if (transform.position.y < -15) // respawn if player goes below -10y
        {
            Respawn();
        }
        

    }
        private void Respawn()
        {
            transform.position = respawnPoint.position;
            rb.velocity = Vector2.zero; // Reset the velocity

            // Apply a small upward force to avoid getting stuck in the platform
            rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }

}
