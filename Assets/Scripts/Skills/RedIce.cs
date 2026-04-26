using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Red/Ice")]
public class RedIce : SkillBase
{
    public int damageDecrease;
    public override void Apply(Lights_Base light)
    {
        light.iceEnabled = true;
        light.baseDamage -= damageDecrease;
    }

    public override void Remove(Lights_Base light)
    {
        light.iceEnabled = false;
        light.baseDamage += damageDecrease;
    }
}
