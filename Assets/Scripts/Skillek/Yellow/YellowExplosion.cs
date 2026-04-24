using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Yellow/Explosion")]
public class YellowExplosion : SkillBase
{
    public int costIncrease;
    public override void Apply(Lights_Base light)
    {
        light.explosionEnabled = true;
        light.baseEnergyCost += costIncrease;
    }

    public override void Remove(Lights_Base light)
    {
        light.explosionEnabled = false;
        light.baseEnergyCost -= costIncrease;
    }
}
