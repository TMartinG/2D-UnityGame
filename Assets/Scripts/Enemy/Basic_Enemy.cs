using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Enemy : Enemy_Base
{
    // Start is called before the first frame update
    [SerializeField] short HP;
    [SerializeField] short SP;



    protected override void Start()
    {

        enemy_HP = HP;
        enemy_SP = SP;
        useAttackState = true;
        base.Start();
        vision = GetComponent<EnemyVision>();
    }

}
