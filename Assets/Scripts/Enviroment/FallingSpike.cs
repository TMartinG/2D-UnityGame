using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    [SerializeField] float fallDelay = 0.5f;
    [SerializeField] float respawnTime = 2f;
    [SerializeField] int damage = 1;
    Vector2 initialPosition;
    Rigidbody2D rb;

    void Awake()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // ne essen le rögtön
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player_Character>().TakeDamage(damage);
        }
    }

    public IEnumerator FallAndRespawn()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.isKinematic = false; //  essen le
        yield return new WaitForSeconds(respawnTime);
        rb.isKinematic = true; // megáll
        rb.velocity = Vector2.zero; // állítsd meg a mozgást
        transform.position = initialPosition; // állj vissza a helyedre
    }
}
