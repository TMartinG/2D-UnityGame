using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Yellow/FireRate")]
public class YellowFireRate : SkillBase
{
    public float fireRateIncrease;

    public override void Apply(Lights_Base light)
    {
        light.baseFireRate -= fireRateIncrease;
    }

    public override void Remove(Lights_Base light)
    {
        light.baseFireRate += fireRateIncrease;
    }

}
