using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Red/Ice Upgrade")]
public class RedIceUpgrade : SkillBase
{
    public float freezeDurationIncrase;
    public float slowLerpSpeedIncrease;
    public float freezeThresholdDecrease;

    public override void Apply(Lights_Base light)
    {
        light.upgradedIce = true;
        light.freezeDuration += freezeDurationIncrase;
        light.slowLerpSpeed += slowLerpSpeedIncrease;
        light.freezeThreshold -= freezeThresholdDecrease;
    }

    public override void Remove(Lights_Base light)
    {
        light.upgradedIce = false;
        light.freezeDuration -= freezeDurationIncrase;
        light.slowLerpSpeed -= slowLerpSpeedIncrease;
        light.freezeThreshold += freezeThresholdDecrease;
    }
}
