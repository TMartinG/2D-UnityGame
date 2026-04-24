using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light_Yellow : Lights_Base
{

    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectileBasic;
    [SerializeField] GameObject projectileExplosion;
    [SerializeField] GameObject projectileBig;
    [SerializeField] GameObject projectileBigExplosion;



    private GameObject bullet;
    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") 
                                    && Time.time >= nextFireTime 
                                    && playerCharacter.playerYellowCurrentEnergy >= baseEnergyCost && playerMovement.openPanels == 0)
        {
            if (playerCharacter.UseYellowEnergy(baseEnergyCost))
            {
                if (doubleEnabled == false)
                {
                    Shoot();
                }
                else
                {
                    StartCoroutine(DoubleShootWithDelay());
                }

                nextFireTime = Time.time + baseFireRate;
            }
           
        }
    }

    // Hatás az ellenfélre
    IEnumerator DoubleShootWithDelay()
    {
        Shoot();
        yield return new WaitForSeconds(0.2f);
        Shoot();
    }

    void Shoot()
    {
        if (bigEnabled == false)
        {
            if (explosionEnabled == false)
            {
                bullet = Instantiate(projectileBasic, firePoint.position, Quaternion.identity);
            }
            else
            {
                bullet = Instantiate(projectileExplosion, firePoint.position, Quaternion.identity);
            }
        }
        else
        {
            if (explosionEnabled == false)
            {
                bullet = Instantiate(projectileBig, firePoint.position, Quaternion.identity);
            }
            else
            {
                bullet = Instantiate(projectileBigExplosion, firePoint.position, Quaternion.identity);
            }    
        }

        // Lövés iránya az egér felé
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (target - (Vector2)firePoint.position).normalized;

        Atk_Y atk = bullet.GetComponent<Atk_Y>();
        atk.Setup(direction);
        atk.Init(this);


    }

    public override void ApplyEffect(GameObject target, float amount)
    {
        
    }
    
}
