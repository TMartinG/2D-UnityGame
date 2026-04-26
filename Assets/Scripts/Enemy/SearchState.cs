using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : IEnemyState
{
    Enemy_Base enemy;

    float waitTimer = 0f;
    float waitTime = 3f;

    float stuckTimer = 0f;
    float stuckTime = 5f;

    Vector2 lastPosition;
    float moveCheckTimer = 0.5f;
    float moveCheckInterval = 0.05f;

    float minMoveDistance = 0.1f;

    public SearchState(Enemy_Base enemy)
    {
        this.enemy = enemy;
        enemy.enemy_State = EnemyState.searching;
    }

    public void Enter()
    {
        waitTimer = 0f;
        stuckTimer = 0f;

        lastPosition = enemy.transform.position;
        moveCheckTimer = 0f;
        enemy.animator.SetBool("isMoving", true);
        
    }

    public void Update()
    {
        // látja -> chase
        if (enemy.vision.CanSeeTarget(enemy.player))
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
            //Debug.Log("Chase state -re váltás");
            return;
        }
        // ha odaért → vár egy kicsit
        //Debug.Log("Keres... Távolság az utolsó látot hejtől: " + Vector2.Distance(enemy.transform.position, enemy.lastSeenPosition).ToString("F2") + " units");
        if (Vector2.Distance(enemy.transform.position, enemy.lastSeenPosition) < 2f)
        {
            waitTimer += Time.deltaTime;
            //Debug.Log("Vár az utoljára látott helyen: " + waitTimer.ToString("F2") + "s");

            if (waitTimer >= waitTime)
            {
                enemy.hasLastSeenPosition = false;
                enemy.stateMachine.ChangeState(enemy.patrolState);
                //Debug.Log("Patrol State -re váltás");
            }
        }

        moveCheckTimer += Time.deltaTime;

        if (moveCheckTimer >= moveCheckInterval)
        {
            float movedDistance = Vector2.Distance(enemy.transform.position, lastPosition);

            if (movedDistance < minMoveDistance)
            {
                // ha nem mozog → stuck
                stuckTimer += moveCheckInterval;
            }
            else
            {
                // mozgott → reset stuck
                stuckTimer = 0f;
            }

            lastPosition = enemy.transform.position;
            moveCheckTimer = 0f;
        }

        stuckTimer += Time.deltaTime;
        if (stuckTimer >= stuckTime)        {
            enemy.hasLastSeenPosition = false;
            enemy.stateMachine.ChangeState(enemy.patrolState);
            //Debug.Log("Beragadt -> Váltás Patrolra");
        }
    }

    public void FixedUpdate()
    {
        if (enemy.hasLastSeenPosition && enemy.enemy_Type == EnemyType.basic)
        {
            enemy.movement?.MoveTo(enemy.lastSeenPosition);
        }
        else if (enemy.hasLastSeenPosition && enemy.enemy_Type == EnemyType.flying)
        {
            
            enemy.movement?.MoveTo_Search(enemy.lastSeenPosition);
        }

    }

    public void Exit() { }
}
