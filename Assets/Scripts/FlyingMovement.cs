using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
public class FlyingMovement : MonoBehaviour, IMovement
{

    [SerializeField] float maxDistanceFromHome;
    [SerializeField] Vector2 homePosition;

    public LayerMask obstacleLayer;
    NavMeshAgent agent;

    void Awake()
    {
        homePosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; //  ne forgassa a 3D-s
        agent.updateUpAxis = false; //  ne állítsa a Y-t felfelé
    }

    //  PATROL / sima mozgás
    public void MoveTo(Vector2 targetPosition)
    {
        float distFromHome = Vector2.Distance(transform.position, homePosition);

        if (distFromHome > maxDistanceFromHome)
        {
            agent.SetDestination(homePosition);
            return;
        }
        agent.SetDestination(targetPosition);
        Flip(agent.velocity.x);
    }

    public void MoveTo_Search(Vector2 targetPosition)
    {
            agent.SetDestination(targetPosition);
            Flip(agent.velocity.x);
    }

    //  ATTACK (távolságtartás)
    public void MaintainDistance(Vector2 targetPosition, float keepDistance, float attackRange)
    {
        float dist = Vector2.Distance(transform.position, targetPosition);

        if (dist < keepDistance)
        {
            //  menj hátrébb
            Vector2 dir = ((Vector2)transform.position - targetPosition).normalized;
            Vector2 newPos = (Vector2)transform.position + dir * 2f;

            agent.SetDestination(newPos);
        }
        else if (dist > attackRange)
        {
            //  menj közelebb
            agent.SetDestination(targetPosition);
        }
        else
        {
            //  állj meg
            agent.ResetPath();
        }

        Flip(agent.velocity.x);
    }

    void Flip(float directionX)
    {
        if (Mathf.Abs(directionX) < 0.1f) return;
        if (directionX > 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    

}
