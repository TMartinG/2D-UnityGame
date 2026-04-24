using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Red/MoveSpeed")]
public class RedMoveSpeed : SkillBase
{
    public float moveSpeedIncrease;
    public float headMoveSpeedIncrease;
    public float jumpForceIncrease;
    public float fallMultiplierIncrease;
    public float lowJumpMultiplierIncrease;


    public override void Apply(Lights_Base light)
    {
        light.betterMovementEnabled = true;
        light.playerMovement.maxMoveSpeed += moveSpeedIncrease;
        light.playerMovement.maxHeadMoveSpeed += headMoveSpeedIncrease;
        light.playerMovement.maxJumpForce += jumpForceIncrease;
        light.playerMovement.maxFallMultiplier += fallMultiplierIncrease;
        light.playerMovement.maxLowJumpMultiplier += lowJumpMultiplierIncrease;
    }

    public override void Remove(Lights_Base light)
    {
        light.betterMovementEnabled = false;
        light.playerMovement.maxMoveSpeed -= moveSpeedIncrease;
        light.playerMovement.maxHeadMoveSpeed -= headMoveSpeedIncrease;
        light.playerMovement.maxJumpForce -= jumpForceIncrease;
        light.playerMovement.maxFallMultiplier -= fallMultiplierIncrease;
        light.playerMovement.maxLowJumpMultiplier -= lowJumpMultiplierIncrease;
    }
}
