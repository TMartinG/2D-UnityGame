using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Scripting.APIUpdating;

public class Atk_Y : MonoBehaviour
{

    [Header("Mozgásbeállítások")]
    //public float baseSpeed = 20f;
    public float lifetime = 2f;

    [Header("Sebesség és pálya görbék")]
    public AnimationCurve speedCurve = AnimationCurve.Linear(0, 1, 1, 1); // alap: konstans sebesség
    public AnimationCurve pathCurve = AnimationCurve.Linear(0, 0, 1, 0);  // alap: nincs görbület

    [Header("Sebesség és pálya görbék")]
    public GameObject explosion;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float elapsedTime;
    private Light_Yellow light_Yellow;
    int reflections = 0;
    int maxReflections = 5;
    public void Init(Light_Yellow shooter)
    {
        light_Yellow = shooter;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(Vector2 direction)
    {
        moveDirection = direction.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // 0–1 közötti arány az élettartam alapján
        float t = Mathf.Clamp01(elapsedTime / lifetime);

        // Sebesség módosítása a görbe alapján
        float curveSpeed = light_Yellow.GetSpeed() * speedCurve.Evaluate(t);

        // Pálya eltérítés a görbe alapján (pl. hullámmozgás)
        Vector2 perpendicular = new Vector2(-moveDirection.y, moveDirection.x);
        float curveOffset = pathCurve.Evaluate(t);

        // Új mozgásirány + sebesség
        Vector2 newVelocity = (moveDirection + perpendicular * curveOffset).normalized * curveSpeed;
        rb.velocity = newVelocity;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") )
        {
            Explode();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Explode();
            collision.gameObject.GetComponent<Enemy_Base>().GetDamaged((short)light_Yellow.GetDamage(), light_Yellow.transform.position);
            //Debug.Log("Enemy hit by Yellow Attack" + light_Yellow.transform.position);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Mirror"))
        {
            if (reflections >= maxReflections)
            {
                Explode();
                return;
            }

            reflections++;

            // Számoljuk a reflektált irányt
            Vector2 incoming = rb.velocity.normalized;
            Vector2 normal = collision.contacts[0].normal.normalized;
            Vector2 reflectDir = Vector2.Reflect(incoming, normal).normalized;
            moveDirection = reflectDir;

            // Vizuálisan forgassuk a sprite-ot is
            float angle = Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
    }

    void Explode()
    {
        GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
        if (light_Yellow.explosionEnabled == true)
        {
            LightExplosion_Damage explosionDamage = explosionInstance.GetComponent<LightExplosion_Damage>();
            if (explosionDamage != null)
            explosionDamage.Init(light_Yellow);
        }

        Destroy(explosionInstance, 0.5f);

        Destroy(gameObject);
    }

}
