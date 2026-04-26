using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Red/Damage")]
public class RedDMG : SkillBase
{
    public int damageIncrease;

    public override void Apply(Lights_Base light)
    {
        light.baseDamage += damageIncrease;
    }

    public override void Remove(Lights_Base light)
    {
        light.baseDamage -= damageIncrease;
    }
}
