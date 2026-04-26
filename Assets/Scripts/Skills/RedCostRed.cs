using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Red/Cost")]
public class RedCost : SkillBase
{
    public int costReduction;

    public override void Apply(Lights_Base light)
    {
        light.baseEnergyCost -= costReduction;
    }

    public override void Remove(Lights_Base light)
    {
        light.baseEnergyCost += costReduction;
    }
}
