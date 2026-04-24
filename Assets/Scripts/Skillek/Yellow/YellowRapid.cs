using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Yellow/Rapid")]
public class YellowRapid : SkillBase
{
   public int costDecrease;
   public float fireRateIncrease;
   public int speedIncrease;
   public int damageDecrease;
    public override void Apply(Lights_Base light)
    {
        light.baseEnergyCost -= costDecrease;
        light.baseFireRate -= fireRateIncrease;
        light.baseSpeed += speedIncrease;
        light.baseDamage -= damageDecrease;
    }

    public override void Remove(Lights_Base light)
    {
        light.baseEnergyCost += costDecrease;
        light.baseFireRate += fireRateIncrease;
        light.baseSpeed -= speedIncrease;
        light.baseDamage += damageDecrease;
    }
}
