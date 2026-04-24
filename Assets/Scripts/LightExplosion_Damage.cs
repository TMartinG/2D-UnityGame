using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightExplosion_Damage : MonoBehaviour
{
    [SerializeField] CircleCollider2D circle_Collider;
    private Light_Yellow light_Yellow;
    public void Init(Light_Yellow shooter)
    {
        light_Yellow = shooter;
    }

    void Start()
    {
        circle_Collider.radius = 0.0001f;
    }

    void Update()
    {
       
        if (light_Yellow == null)
        {
            Destroy(gameObject);
            return;
        }
        while (circle_Collider.radius < light_Yellow.explosionMaxRadius)
        {
            circle_Collider.radius += Time.deltaTime * 10;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy_Base>().GetDamaged((short)(light_Yellow.GetDamage() * light_Yellow.explosionMultiplier), light_Yellow.transform.position);
            Debug.Log("Enemy hit");
        }
        
    }
}
