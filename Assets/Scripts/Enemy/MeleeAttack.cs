using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttack
{
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] int damage = 1;

    float timer;
    Enemy_Base enemyBase;
    void Start()
    {
        enemyBase = GetComponent<Enemy_Base>();
    }

    public void Attack(Transform target)
    {
        timer += Time.deltaTime;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= attackRange && timer >= attackCooldown)
        {
            Player_Character player = target.GetComponent<Player_Character>();

            if (player != null)
            {
                enemyBase.animator.SetTrigger("attack");
                player.TakeDamage(damage);
            }

            timer = 0f;
        }
    }
}
