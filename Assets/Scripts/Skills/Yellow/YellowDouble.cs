using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Yellow/Double")]
public class YellowDouble : SkillBase
{
    public int costIncrease;
    public override void Apply(Lights_Base light)
    {
        light.doubleEnabled = true;
        light.baseEnergyCost += costIncrease;
    }

    public override void Remove(Lights_Base light)
    {
        light.doubleEnabled = false;
        light.baseEnergyCost -= costIncrease;
    }
}
