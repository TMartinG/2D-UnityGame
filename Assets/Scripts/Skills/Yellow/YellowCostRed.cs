using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Yellow/Cost")]
public class YellowCostRed : SkillBase
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
