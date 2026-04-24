using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Red/Fire Upgrade")]
public class RedFireUpgrade : SkillBase
{
    public short fireDamageIncrease;
    public float fireDurationIncrease;

    public override void Apply(Lights_Base light)
    {
        light.upgradedFire = true;
        light.fireDamage += fireDamageIncrease;
        light.fireDuration += fireDurationIncrease;
    }

    public override void Remove(Lights_Base light)
    {
        light.upgradedFire = false;
        light.fireDamage -= fireDamageIncrease;
        light.fireDuration -= fireDurationIncrease;
    }

}
