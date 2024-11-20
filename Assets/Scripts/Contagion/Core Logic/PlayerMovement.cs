using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public Rigidbody2D rb;

    private Vector2 movement;
    private Vector2 lastMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize movement vector
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // Store last non-zero movement for facing direction
        if (movement != Vector2.zero)
        {
            lastMovement = movement;
        }
    }

    void FixedUpdate()
    {
        // Move the player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
