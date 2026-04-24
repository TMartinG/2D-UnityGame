using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapChunk : MonoBehaviour
{
    public Transform[] spawnPoints;
    public int chunkIndex;

    bool spawned = false;

    void Start()
    {
        SpawnSkills();
    }

    void SpawnSkills()
    {
        if (spawned) return;

        var skills = SkillModes.Instance.chunkSkills[chunkIndex];

        // Debug.Log($"Chunk {chunkIndex} skill szám: {skills.Count}");

        for (int i = 0; i < skills.Count; i++)
        {
            if (i >= spawnPoints.Length)
            {
                // Debug.LogWarning($"Chunk {chunkIndex} nincs elég spawnPoint!");
                break;
            }

            GameObject obj = Instantiate(
                skills[i],
                spawnPoints[i].position,
                Quaternion.identity
            );
            obj.transform.SetParent(transform);
        }

        spawned = true;
    }
}
