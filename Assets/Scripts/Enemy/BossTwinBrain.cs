using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTwinBrain : MonoBehaviour
{   
    [Header("Bossok")]
    [SerializeField] Boss_Two twin1;
    [SerializeField] Boss_Two twin2;

    [Header("Wave Spawner")]
    [SerializeField] BossWaveSpawner spawner;

    [Header("Reward")]
    [SerializeField] GameObject orangeKey;
    [SerializeField] Transform keySpawnPoint;
    [SerializeField] GameObject door;

    [Header("Camera")]
    [SerializeField] Camera_Change cameraChange;
    public int bossIndex; // 0 vagy 1

    public float maxTotalHP;

    bool spawned70 = false;
    bool spawned50 = false;
    bool spawned20 = false;

    void Start()
    {
        Boss_Two[] bosses = FindObjectsOfType<Boss_Two>();

        if (bosses.Length >= 2)
        {
            twin1 = bosses[0];
            twin2 = bosses[1];
        }
        if (WorldStateManager.Instance.IsDead(twin1.uniqueID) && WorldStateManager.Instance.IsDead(twin2.uniqueID))
        {
            Destroy(twin1.gameObject);
            Destroy(twin2.gameObject);
            return;
        }

        maxTotalHP = twin1.maxHP + twin2.maxHP;
        
        door = GameObject.Find("BOSSMASODIK");
        door.SetActive(false);
    }

    void Update()
    {
        HandlePhases();
        CheckDeath();
        CheckDoor();
    }

    void HandlePhases()
    {
        float currentHP = Mathf.Max(0, twin1.enemy_HP) + Mathf.Max(0, twin2.enemy_HP);
        float percent = currentHP / maxTotalHP;
        //Debug.Log("currentHp: " + currentHP + "MaXTotal: " + maxTotalHP + "percentage: "+ percent);

        if (percent <= 0.7f && !spawned70)
        {
            spawner.SpawnWave(0);
            spawned70 = true;
        }

        if (percent <= 0.5f && !spawned50)
        {
            spawner.SpawnWave(1);
            spawned50 = true;
        }

        if (percent <= 0.2f && !spawned20)
        {
            spawner.SpawnWave(2);
            spawned20 = true;
        }
    }

    void CheckDeath()
    {
        
        if (twin1 == null && twin2 == null)
        {
            if (twin1.isDead && twin2.isDead) return;
            twin1.isDead = true;
            twin2.isDead = true;
            WorldStateManager.Instance.RegisterDead(twin1.uniqueID);
            WorldStateManager.Instance.RegisterDead(twin2.uniqueID);

            cameraChange.camBack();
            SpawnReward();
            enabled = false;
        }
    }

    void SpawnReward()
    {
        Instantiate(orangeKey, keySpawnPoint.position, Quaternion.identity, transform.parent);
        door.SetActive(true);
        var skill = SkillModes.Instance.bossDrops[bossIndex];

        if (skill != null)
        {
            Instantiate(skill, transform.position, Quaternion.identity, transform.parent);
        }
    }

    public void CheckDoor()
    {
        if (door == null)
        {
            door = GameObject.Find("BOSSMASODIK");
        }
    }

}
