using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_One : Enemy_Base
{
    [SerializeField] GameObject key;
    [SerializeField] GameObject door;
    [SerializeField] short HP;
    [SerializeField] short SP;

    [SerializeField] MonoBehaviour meleeAttackComponent;
    [SerializeField] MonoBehaviour rangedAttackComponent;

    [HideInInspector] public IAttack meleeAttack;
    [HideInInspector] public IAttack rangedAttack;

    [SerializeField] BossWaveSpawner waveSpawner;
    [SerializeField] GameObject cameraChangeObj;
    private Camera_Change camChange;

    // százalékok
    float[] spawnThresholds = { 0.9f, 0.7f, 0.5f, 0.3f, 0.1f };

    // hogy melyik már lefutott
    bool[] triggered;

    public int bossIndex; // 0 vagy 1

    void Awake()
    {
        camChange = cameraChangeObj.GetComponent<Camera_Change>();
    }
    protected override void Start()
    {
        enemy_HP = HP;
        enemy_SP = SP;
        useAttackState = true;

        base.Start();

        meleeAttack = meleeAttackComponent as IAttack;
        rangedAttack = rangedAttackComponent as IAttack;


        vision = GetComponent<EnemyVision>();
        triggered = new bool[spawnThresholds.Length];

        door = GameObject.Find("BOSS1");
        door.SetActive(false);

    }
    protected override void Update()
    {
        base.Update();

        CheckWaveSpawns();
    }
    public override void Die()
    {
        if (isDead) return;
        isDead = true;

        Destroy(gameObject);
        camChange.camBack();
        DropKey();
        DropSkillMode();
        DropHealthPickup();
        door.SetActive(true);
    }

    public void DropKey()
    {
        Instantiate(key, transform.position, Quaternion.identity, transform.parent);
    }
    void CheckWaveSpawns()
    {
        float hpPercent = (float)enemy_HP / HP;

        for (int i = 0; i < spawnThresholds.Length; i++)
        {
            if (!triggered[i] && hpPercent <= spawnThresholds[i])
            {
                triggered[i] = true;

                //Debug.Log("Wave spawn at: " + (spawnThresholds[i] * 100) + "%");

                waveSpawner.SpawnWave(i);
            }
        }
    }

    void DropSkillMode()
    {
        var skill = SkillModes.Instance.bossDrops[bossIndex];

        if (skill != null)
        {
            Instantiate(skill, transform.position, Quaternion.identity, transform.parent);
        }

    }

}
