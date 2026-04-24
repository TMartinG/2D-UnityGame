using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    Enemy_Base enemy;

    public IdleState(Enemy_Base enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.enemy_State = EnemyState.idle;
        //Debug.Log("Idle");
        enemy.animator.SetBool("isMoving", false);
    }

    public void Update()
    {

        if (enemy.vision.CanSeeTarget(enemy.player))
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
        }
        else
        {
            enemy.stateMachine.ChangeState(enemy.patrolState);
        }
    }
    public void FixedUpdate()
    {
        
    }

    public void Exit()
    {

    }
}
