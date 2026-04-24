using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_Enemy : Enemy_Base
{
    [SerializeField] short HP;
    [SerializeField] short SP;



    protected override void Start()
    {
        enemy_HP = HP;
        enemy_SP = SP;
        //ApplyState(HP);
        useAttackState = true;
        base.Start();
        vision = GetComponent<EnemyVision>();
    }

}
