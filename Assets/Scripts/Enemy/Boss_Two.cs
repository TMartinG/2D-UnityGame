using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Two : Enemy_Base
{
    
    public short maxHP;

    [HideInInspector] public IAttack rangedAttack;
    [SerializeField] FlyingRangedAttack flyingRangedAttack;

    [SerializeField] float secondPfireRate;
    [SerializeField] float secondPbulletSpeed;


   protected override void Start()
    {

        //enemy_HP = WorldStateManager.Instance.GetHealth(uniqueID, maxHP);

        enemy_HP = maxHP;

        useAttackState = true;

        base.Start();

        rangedAttack = flyingRangedAttack as IAttack;

        vision = GetComponent<EnemyVision>();

    }

    protected override void Update()
    {
        base.Update();

        isSecondPhase();
    }

    public override void Die()
    {
        Destroy(gameObject);
    }

    public void isSecondPhase()
    {
        float hpPercent = (float)enemy_HP / maxHP;

        if (hpPercent <= 0.5f && flyingRangedAttack.attackType== FlyingRangedAttack.AttackType.Double)
        {
            flyingRangedAttack.fireRate =secondPfireRate;
            flyingRangedAttack.bullet.GetComponent<Enemy_Bullet>().speed = secondPbulletSpeed;
        }
        else if(hpPercent <= 0.5f && flyingRangedAttack.attackType== FlyingRangedAttack.AttackType.Spread3)
        {
            flyingRangedAttack.fireRate = secondPfireRate;
            flyingRangedAttack.attackType = FlyingRangedAttack.AttackType.Spread5;
        }
    }


}
