using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWaveSpawner : MonoBehaviour
{
    [Header("Wave Configs")]
    [SerializeField] List<Wave> bossWaves;

    List<GameObject> aliveEnemies = new List<GameObject>();

    public void SpawnWave(int index)
    {
        if (index < 0 || index >= bossWaves.Count)
        {
            return;
        }

        Wave wave = bossWaves[index];

        foreach (var group in wave.groups)
        {
            for (int i = 0; i < group.count; i++)
            {
                GameObject enemy = Instantiate(group.enemyPrefab, group.spawnPoint.position, Quaternion.identity);
                enemy.transform.SetParent(transform);

                aliveEnemies.Add(enemy);

                Enemy_Base enemyB = enemy.GetComponent<Enemy_Base>();
                if (enemyB != null)
                {
                    enemyB.OnDeath += () => aliveEnemies.Remove(enemy);
                }
            }
        }
    }

    public bool AreEnemiesAlive()
    {
        return aliveEnemies.Count > 0;
    }
    
}
