using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    [SerializeField] List<Wave> waves;
    [SerializeField] float timeBetweenWaves = 2f;

    int currentWave = 0;
    List<GameObject> aliveEnemies = new List<GameObject>();
    bool started = false;

    [SerializeField]  Player_Movement player_Movement;
    [SerializeField]  Animator animator;
    public float cameraSizeChange = 40f;
    public float cameraSizeDefault = 15f;


    void Start()
    {
        player_Movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !started)
        {
            player_Movement.GrowToTarget(cameraSizeChange, 2f);
            player_Movement.maxCameraSize = cameraSizeChange;
            Debug.Log("Wave spawner started!");
            started = true;
            StartCoroutine(RunWaves());
        }
    }

    IEnumerator RunWaves()
    {
        while (currentWave < waves.Count)
        {
            SpawnWave(waves[currentWave]);

            yield return new WaitUntil(() => aliveEnemies.Count == 0);

            currentWave++;

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        player_Movement.GrowToTarget(cameraSizeDefault, 2f);
        player_Movement.maxCameraSize = cameraSizeDefault;
        animator.SetTrigger("canGo");
        //Debug.Log("Minden wave kész!");
    }

    void SpawnWave(Wave wave)
    {
        foreach (var group in wave.groups)
        {
            for (int i = 0; i < group.count; i++)
            {
                GameObject enemy = Instantiate(group.enemyPrefab, group.spawnPoint.position, Quaternion.identity);

                aliveEnemies.Add(enemy);

                Enemy_Base enemyB = enemy.GetComponent<Enemy_Base>();
                if (enemyB != null)
                {
                    enemyB.OnDeath += () => aliveEnemies.Remove(enemy);
                }
            }
        }
    }
}
