using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Red : Lights_Base
{

    [Header("Refs")]
    [SerializeField] Red_Aim[] player_Aim_UpgradedArray;
    private bool isFiring = false;


    void Update()
    {
        if (!gameObject.activeInHierarchy) return; // csak ha aktív

        if (playerMovement.openPanels == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                StartFiring();

            }
            if (Input.GetButton("Fire1"))
            {
                if (isFiring)
                {
                    float energyDrain = baseEnergyCost * Time.deltaTime;

                    if (playerCharacter.UseRedEnergy(energyDrain))
                    {
                        player_Aim_UpgradedArray[WichLaser()].UpdateLaser();
                    }
                    else
                    {
                        StopFiring();
                    }
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                StopFiring();
            }
            player_Aim_UpgradedArray[WichLaser()].RotateMouse();
        }
        
    }

    public override void ApplyEffect(GameObject target, float amount )
    {
        Enemy_Base enemy = target.GetComponent<Enemy_Base>();
        if (enemy != null)
        {
            if (iceEnabled == true)
            {
                enemy.ApplyDebuff(DebuffType.freeze, amount, 0, 0, slowLerpSpeed, freezeThreshold, freezeDuration);
            }
            if (fireEnabled == true)
            {
                enemy.ApplyDebuff(DebuffType.fire, amount, fireDamage, fireDuration, 0, 0, 0);
                
            }
        }
    }

    private void StartFiring()
    {
        //Debug.Log("Wich laser: " + WichLaser());
        player_Aim_UpgradedArray[WichLaser()].ResetLaser();
        isFiring = true;
        player_Aim_UpgradedArray[WichLaser()].lineRenderer.enabled = true;
        playerMovement.ShootingRed();
    }

    private void StopFiring()
    {
        isFiring = false;
        player_Aim_UpgradedArray[WichLaser()].lineRenderer.enabled = false;
        player_Aim_UpgradedArray[WichLaser()].StopVFX();

        playerMovement.ResetMovementRed();
        player_Aim_UpgradedArray[WichLaser()].ResetLaser();
    }


    public override void Deactivate()
    {
        StopFiring();
        gameObject.SetActive(false);
        if (light2DFlash != null)
        {
            light2DFlash.pointLightOuterAngle = 130f;
            light2DFlash.pointLightInnerAngle = 80f;
        }
    }
    

}
