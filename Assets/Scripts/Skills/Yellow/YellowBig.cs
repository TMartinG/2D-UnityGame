using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Yellow/Big")]
public class YellowBig : SkillBase
{
    public int costIncrease;
    public float fireRateDecrease;
    public int damageIncrease;
    public float explosionMaxRadiusIncrease;
    public float explosionMultiplierIncrease;

    public override void Apply(Lights_Base light)
    {
        light.bigEnabled = true;
        light.baseEnergyCost += costIncrease;
        light.baseFireRate += fireRateDecrease;
        light.baseDamage += damageIncrease;
        light.explosionMaxRadius += explosionMaxRadiusIncrease;
        light.explosionMultiplier += explosionMultiplierIncrease;
    }

    public override void Remove(Lights_Base light)
    {
        light.bigEnabled = false;
        light.baseEnergyCost -= costIncrease;
        light.baseFireRate -= fireRateDecrease;
        light.baseDamage -= damageIncrease;
        light.explosionMaxRadius -= explosionMaxRadiusIncrease;
        light.explosionMultiplier -= explosionMultiplierIncrease;
    }
}
