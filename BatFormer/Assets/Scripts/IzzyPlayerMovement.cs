using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IzzyPlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of the player movement
    public float jumpForce = 5f; // Force of the jump
    public float wallJumpForce = 5f; // Force applied for wall jumps. While we'll likely keep these 2 the same, it's good to have the option
    private Rigidbody2D rb;
    //private bool isGrounded = false; // obsolete from earlier versions of the jump, but still could be used in future?
    public int maxJumps = 2; // The editable max number of jumps, scales infintely
    private int numberOfJumps; //Tracking current number of jumps
    private bool isTouchingWall; // New variable to track wall contact
    public LayerMask whatIsWall; // Define what layers count as walls in the inspector
    private bool isJumpingOffWall; // To prevent sticking to the wall after jumping, tbh idk why I need this, but here it is lol


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
        numberOfJumps = maxJumps;
    }

    void Update()
    {
        // Horizontal movement
        float moveHorizontal = Input.GetAxis("Horizontal") * speed;
        rb.velocity = new Vector2(moveHorizontal, rb.velocity.y);

        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (numberOfJumps > 0 && !isTouchingWall)
            {
                PerformJump();
            }
            else if (isTouchingWall && !isJumpingOffWall)
            {
                PerformWallJump();
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumpingOffWall = false; // Reset wall jump status
        }

    }
    //jump function
    private void PerformJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0); //sets vertical movement to zero so physics of the jump is the same no matter vertical momentum. Just improves game feel in general
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        numberOfJumps--;
    }

    private void PerformWallJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vertical movement
        rb.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpForce, jumpForce), ForceMode2D.Impulse);
        isJumpingOffWall = true; // Prevent sticking to the wall
        numberOfJumps = maxJumps - 1; // Deduct one jump as wall jump is used
    }

    // Check for when the player hits the ground or wall
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Make sure any ground has a tag "Ground" in the inspector or else this breaks and we all die :(
        {
            //isGrounded = true;
            numberOfJumps = maxJumps;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
        }
    }
   

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //isGrounded = false;

        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
        }
    }
}

