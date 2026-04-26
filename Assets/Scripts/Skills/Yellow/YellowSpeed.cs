using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Yellow/Speed")]
public class YellowSpeed : SkillBase
{

    public float speedIncrease;

    public override void Apply(Lights_Base light)
    {
        light.baseSpeed += speedIncrease;
    }

    public override void Remove(Lights_Base light)
    {
        light.baseSpeed -= speedIncrease;
    }
}
