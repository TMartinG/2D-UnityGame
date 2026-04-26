using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public float viewDistance = 6f;
    public float viewAngle = 90f;
    public LayerMask obstacleLayer;

    public bool CanSeeTarget(Transform target)
    {
        Vector2 directionToTarget = (target.position - transform.position);
        float distance = directionToTarget.magnitude;

        if (distance > viewDistance)
            return false;

        float angle = Vector2.Angle(transform.right, directionToTarget);

        if (angle > viewAngle / 2f)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            directionToTarget.normalized,
            distance,
            obstacleLayer
        );

        if (hit.collider != null)
        {
            // fal van közte
            return false;
        }

        return true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 left = Quaternion.Euler(0, 0, viewAngle / 2) * transform.right;
        Vector3 right = Quaternion.Euler(0, 0, -viewAngle / 2) * transform.right;

        Gizmos.DrawLine(transform.position, transform.position + left * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + right * viewDistance);
    }

}
