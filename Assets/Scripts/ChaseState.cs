using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    Enemy_Base enemy;

    public ChaseState(Enemy_Base enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.enemy_State = EnemyState.chasing;
        //Debug.Log("Chase");
        enemy.animator.SetBool("isMoving", true);
    }

    public void Update()
    {
        if (enemy.vision.CanSeeTarget(enemy.player))
        {
            //látja → mentjük a pozíciót
            enemy.lastSeenPosition = enemy.player.position;
            enemy.hasLastSeenPosition = true;

            enemy.chaseLostSightTimer = 0f;
        }
        else
        {
            enemy.chaseLostSightTimer += Time.deltaTime;

            if (enemy.chaseLostSightTimer >= enemy.chaseLostSightDelay)
            {
                enemy.stateMachine.ChangeState(enemy.searchState);
                //Debug.Log("Switching to Search State");
                return;
            }
        }

        if (enemy.useAttackState && enemy.DistanceToPlayer() <= enemy.chaseAttackRange)
        {
            enemy.stateMachine.ChangeState(enemy.attackState);
        }
    }
    
    public void FixedUpdate()
    {
        if (enemy.enemy_Type == EnemyType.basic || enemy.enemy_Type == EnemyType.boss1)
        {
            enemy.movement?.MoveTo(enemy.player.position);
        }
        else if (enemy.enemy_Type == EnemyType.flying)
        {
            enemy.movement?.MoveTo_Search(enemy.player.position);
        }

    }

    public void Exit()
    {

    }
}
