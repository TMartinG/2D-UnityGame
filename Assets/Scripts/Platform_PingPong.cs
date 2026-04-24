using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_PingPong : MonoBehaviour
{
    Vector3 startPos;
    Rigidbody2D rb;

    [SerializeField] float speed = 2f;
    [SerializeField] float distance = 3f;

    [SerializeField] Vector2 direction = Vector2.right;

    public Vector2 platformVelocity;
    Vector3 lastPos;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();

        direction = direction.normalized;
    }

    void FixedUpdate()
    {
        platformVelocity = (transform.position - lastPos) / Time.fixedDeltaTime;
        lastPos = transform.position;

        float t = Mathf.PingPong(Time.time * speed, distance);

        rb.MovePosition(startPos + (Vector3)(direction * t));
    }
}
