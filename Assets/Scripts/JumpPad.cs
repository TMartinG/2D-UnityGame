using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            Player_Movement player = collision.GetComponent<Player_Movement>();

            if (rb != null && player != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                
                player.isJumpPadBoost = true;
            }
        }
    }
}
