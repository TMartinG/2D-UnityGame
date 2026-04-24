using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossOne_GroundMovement : MonoBehaviour, IMovement
{
    [SerializeField] float jumpForceX = 8f;
    [SerializeField] float jumpForceY = 10f;
    [SerializeField] float jumpCooldown = 2f;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;

    Rigidbody2D rb;

    float jumpTimer;
    bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGround();
        jumpTimer += Time.deltaTime;
    }

    public void MoveTo(Vector2 targetPosition)
    {
        // csak akkor ugrik ha földön van
        if (!isGrounded) return;

        if (jumpTimer >= jumpCooldown)
        {
            JumpTowards(targetPosition);
            jumpTimer = 0f;
        }
    }

    public void MoveTo_Search(Vector2 targetPosition)
    {
        MoveTo(targetPosition); 
    }

    void JumpTowards(Vector2 target)
    {
        Vector2 dir = (target - rb.position);

        float directionX = Mathf.Sign(dir.x);

        rb.velocity = new Vector2(0, rb.velocity.y);

        // ugrás
        rb.AddForce(new Vector2(directionX * jumpForceX, jumpForceY), ForceMode2D.Impulse);

        Flip(directionX);
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    void Flip(float directionX)
    {
        if (directionX > 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

}
