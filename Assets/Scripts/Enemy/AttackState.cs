using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    Enemy_Base enemy;
    Boss_One boss_One;
    Boss_Two boss_Two;

    public AttackState(Enemy_Base enemy)
    {
        this.enemy = enemy;
        boss_One = enemy.GetComponent<Boss_One>();
        boss_Two = enemy.GetComponent<Boss_Two>();
    }

    public void Enter()
    {
        enemy.enemy_State = EnemyState.attacking;
    }

    public void Update()
    {

        if (enemy.vision.CanSeeTarget(enemy.player))
        {
            
             float dist = enemy.DistanceToPlayer();

            //  BOSS SPECIFIKUS LOGIKA
            if (enemy.enemy_Type == EnemyType.boss1)
            {
                if (dist <= enemy.attackAttackRange)
                {
                    boss_One.meleeAttack?.Attack(enemy.player);
                }
                else if(dist > enemy.attackAttackRange)
                {
                    boss_One.rangedAttack?.Attack(enemy.player);
                }
            }
            if (enemy.enemy_Type == EnemyType.boss2)
            {
                boss_Two.rangedAttack?.Attack(enemy.player);
            }

            enemy.attack?.Attack(enemy.player);
            enemy.lastSeenPosition = enemy.player.position;
            enemy.hasLastSeenPosition = true;
            enemy.attackLostSightTimer = 0f;
        }
        else
        {
            enemy.attackLostSightTimer += Time.deltaTime;

            if (enemy.attackLostSightTimer >= enemy.chaseLostSightDelay)
            {
                enemy.stateMachine.ChangeState(enemy.searchState);
                return;
            }
        }

        if (enemy.DistanceToPlayer()-enemy.keepDistance > enemy.attackAttackRange)
        {
            //Debug.Log("Distance to Player: " + enemy.DistanceToPlayer());
            //Debug.Log("attack range: " + enemy.attackAttackRange);
            enemy.stateMachine.ChangeState(enemy.chaseState);
        }
    }

    public void FixedUpdate()
    {
        FlyingMovement fm = enemy.movement as FlyingMovement;

        if (fm != null)
        {
            fm.MaintainDistance(enemy.player.position, enemy.keepDistance, enemy.attackAttackRange);
        }
    }

    public void Exit()
    {

    }
}
