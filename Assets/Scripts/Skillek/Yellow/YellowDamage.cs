using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Yellow/Damage")]
public class YellowDamage : SkillBase
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
