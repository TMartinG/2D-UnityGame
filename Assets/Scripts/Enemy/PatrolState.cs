using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    Enemy_Base enemy;

    Vector2 targetPoint;
    float changeTargetTimer;

    public PatrolState(Enemy_Base enemy)
    {
        this.enemy = enemy;
       
    }

    public void Enter()
    {
        enemy.enemy_State = EnemyState.patrolling;
        enemy.animator.SetBool("isMoving", true);

        if (enemy is Flying_Enemy)
        {
            
            PickNewPoint_Fly();
        }
        else
        {
            PickNewPoint();
        }
        
    }

    public void Update()
    {
        changeTargetTimer += Time.deltaTime;


        // ha odaért vagy idő lejárt → új pont
        if (Vector2.Distance(enemy.transform.position, targetPoint) < 0.2f
            || changeTargetTimer > enemy.patrolChangeInterval)
        {
            if (enemy is Flying_Enemy)
            {
                PickNewPoint_Fly();
            }
            else
            {
                PickNewPoint();
            }
        }

        // látás
        if (enemy.vision.CanSeeTarget(enemy.player))
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
        }
    }

    public void FixedUpdate()
    {
        enemy.movement?.MoveTo(targetPoint);
    }

    void PickNewPoint()
    {
        changeTargetTimer = 0f;

        float randomX = Random.Range(-enemy.patrolRadius, enemy.patrolRadius);

        targetPoint = new Vector2(
            enemy.transform.position.x + randomX,
            enemy.transform.position.y
        );
    }
    void PickNewPoint_Fly()
    {
        changeTargetTimer = 0f;

        Vector2 random = Random.insideUnitCircle * enemy.patrolRadius;

        targetPoint = (Vector2)enemy.transform.position + random;
    }

    public void Exit() { }


}
