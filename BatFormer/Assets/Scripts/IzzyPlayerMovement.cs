using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IzzyPlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of the player movement
    public float jumpForce = 5f; // Force of the jump
    private Rigidbody2D rb;
    //private bool isGrounded = false; // Check if the player is on the ground, OBSOLETE
    public int maxJumps = 2; // The editable max number of jumps, scales infintely
    private int numberOfJumps; //Tracking current number of jumps
    

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
        if (Input.GetButtonDown("Jump") && numberOfJumps > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);//sets vertical movement to zero so physics of the jump is the same no matter vertical momentum. Just improves game feel in general
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            numberOfJumps--;
        }
    }

    // Check if the player is grounded
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Make sure your ground has a tag "Ground" in the inspector
        {
            //isGrounded = true;
            numberOfJumps = maxJumps;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //isGrounded = false;
            //this doesn't really do anything anymore, but could be a useful reference
        }
    }
}