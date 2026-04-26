using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRangedAttack : MonoBehaviour, IAttack
{
    [SerializeField] float shootingRange = 5f;
    public float fireRate = 1.5f;

    public GameObject bullet;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] Enemy_Base enemyBase;
    public AttackType attackType;
    float nextFireTime;

    void Start()
    {
        enemyBase = GetComponent<Enemy_Base>();
    }

    public void Attack(Transform target)
    {
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= shootingRange && Time.time > nextFireTime)
        {
            enemyBase.animator.SetTrigger("attack");

            switch (attackType)
            {
                case AttackType.Single:
                    ShootSingle(target);
                    break;

                case AttackType.Spread3:
                    ShootSpread(target, 3, 15f);
                    break;

                case AttackType.Spread5:
                    ShootSpread(target, 5, 10f);
                    break;

                case AttackType.Double:
                    StartCoroutine(DoubleShot(target));
                    break;
            }

            nextFireTime = Time.time + fireRate;
        }
    }

    // Sima lövés
    /*void Shoot(Transform target)
    {
        GameObject b = Instantiate(bullet, bulletSpawn.position, Quaternion.identity);

        Enemy_Bullet bulletScript = b.GetComponent<Enemy_Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetTarget(target);
        }
    }*/

    void ShootSpread(Transform target, int bulletCount, float spreadAngle)
    {
        Vector2 baseDir = (target.position - bulletSpawn.position).normalized;

        float startAngle = -spreadAngle * (bulletCount - 1) / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * spreadAngle;

            Vector3 dir = Quaternion.Euler(0, 0, angle) * baseDir;
            Quaternion rot = Quaternion.FromToRotation(Vector3.right, dir);

            SpawnBullet(dir);
        }
    }

    void SpawnBullet(Vector2 dir)
    {
        GameObject b = Instantiate(bullet, bulletSpawn.position, Quaternion.identity);

        Enemy_Bullet bulletScript = b.GetComponent<Enemy_Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetDirection(dir);
        }
    }

    IEnumerator DoubleShot(Transform target)
    {
        ShootSingle(target);
        yield return new WaitForSeconds(0.3f);
        ShootSingle(target);
    }

    void ShootSingle(Transform target)
    {
        Vector2 dir = (target.position - bulletSpawn.position).normalized;
        SpawnBullet(dir);
    }

    public enum AttackType
{
        Single,
        Spread3,
        Spread5,
        Double
    }


}
