using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{

    Transform target;
    Vector2 direction;
    bool useDirection = false;

    public float speed = 5f;

    Rigidbody2D rb;
    public bool isIgnoreGround = false;
    public float whenToDestroy = 4f;



    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        useDirection = false;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        useDirection = true;
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 dir;

        if (useDirection)
        {
            dir = direction;
        }
        else
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            dir = (target.position - transform.position).normalized;
        }

        rb.velocity = dir * speed;

    //  FORGATÁS
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, whenToDestroy);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       // Debug.Log("Bullet hit: " + other.name);
        Player_Character player = other.GetComponent<Player_Character>();

        if (player != null)
        {
            player.TakeDamage(1);
            Destroy(gameObject);
        }
        else if ((other.CompareTag("Ground") || other.CompareTag("Mirror") || other.CompareTag("Platform") )&& isIgnoreGround == false)
        {
            Destroy(gameObject);
        }
    }

}
