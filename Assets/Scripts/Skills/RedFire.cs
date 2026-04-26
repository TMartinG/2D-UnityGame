using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Red/Fire")]

public class RedFire : SkillBase
{
    public override void Apply(Lights_Base light)
    {
        light.fireEnabled = true;
    }

    public override void Remove(Lights_Base light)
    {
        light.fireEnabled = false;
    }
}
