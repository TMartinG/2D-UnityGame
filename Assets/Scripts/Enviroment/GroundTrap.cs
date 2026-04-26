using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrap : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Character playerHealth = other.GetComponent<Player_Character>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        if(other.CompareTag("Enemy"))
        {
            Enemy_Base enemy = other.GetComponent<Enemy_Base>();
            if (enemy != null)
            {
                enemy.GetDamaged((short)damage);
            }
        }
    }
}
