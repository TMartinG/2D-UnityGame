using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
public class GroundChaseMovement : MonoBehaviour, IMovement
{
    [SerializeField] float speed = 2f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] Vector2 homePosition;
    [SerializeField] float maxDistanceFromHome;

    Rigidbody2D rb;
    Enemy_Base enemyBase;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyBase = GetComponent<Enemy_Base>();
        speed =enemyBase.enemy_SP;
        homePosition = transform.position;
    }


    public void MoveTo(Vector2 targetPosition)
    {
        MoveHorizontal(targetPosition);
    }

    public void MoveTo_Search(Vector2 targetPosition)
    {
        MoveHorizontal(targetPosition);
    }

    void MoveHorizontal(Vector2 targetPosition)
    {
        float minX = homePosition.x - maxDistanceFromHome;
        float maxX = homePosition.x + maxDistanceFromHome;

        // Target clamp
        float clampedTargetX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float distanceX = clampedTargetX - rb.position.x;

        //  Ha közel → lassulj meg
        if (Mathf.Abs(distanceX) < 0.15f)
        {
            rb.velocity = new Vector2(
                Mathf.Lerp(rb.velocity.x, 0, acceleration * Time.fixedDeltaTime),
                rb.velocity.y
            );
            return;
        }

        float directionX = Mathf.Sign(distanceX);

        //  Határ check (ne menjen kifelé)
        if ((rb.position.x <= minX && directionX < 0) ||
            (rb.position.x >= maxX && directionX > 0))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        float targetSpeed = directionX * speed * enemyBase.slowMultiplier;

        // Smooth mozgás
        float newVelX = Mathf.Lerp(rb.velocity.x, targetSpeed, acceleration * Time.fixedDeltaTime);
        rb.velocity = new Vector2(newVelX, rb.velocity.y);

        Flip(directionX);

        // Biztonsági clamp (soha ne csússzon ki)
        Vector2 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        rb.position = pos;
    }

    void Flip(float directionX)
    {
        if (Mathf.Abs(directionX) < 0.2f) return;
        if (directionX > 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
