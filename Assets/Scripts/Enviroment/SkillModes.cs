using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillModes : MonoBehaviour
{
    public static SkillModes Instance;

    public GameObject[] modes; // 8 buff (5(+1) biom + 2 boss)

    public List<List<GameObject>> chunkSkills = new List<List<GameObject>>();

    public GameObject[] bossDrops = new GameObject[2];

    public int[] chunkSpawnCounts; // pl: [1,1,0,2,1,1]

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetupSkills();
    }


    void SetupSkills()
    {
        chunkSkills.Clear();

        Shuffle(modes);

        int chunkCount = chunkSpawnCounts.Length;

        // chunk listák létrehozása
        for (int i = 0; i < chunkCount; i++)
        {
            chunkSkills.Add(new List<GameObject>());
        }

        int index = 0;

        // buffok kiosztása chunkokra spawn szám alapján
        for (int i = 0; i < chunkCount; i++)
        {
            int spawnCount = chunkSpawnCounts[i];

            for (int j = 0; j < spawnCount; j++)
            {
                if (index >= modes.Length - 2)
                {
                    //Debug.LogWarning("Túl sok spawn a chunkokban!");
                    return;
                }

                chunkSkills[i].Add(modes[index]);
                index++;
            }
        }

        // boss dropok (utolsó 2)
        bossDrops[0] = modes[modes.Length - 2];
        bossDrops[1] = modes[modes.Length - 1];
    }
    void Shuffle(GameObject[] array) //Fisher–Yates algoritmus
    {
        for (int i = 0; i < array.Length; i++)
        {
            int rnd = Random.Range(i, array.Length);
            GameObject temp = array[i];
            array[i] = array[rnd];
            array[rnd] = temp;
        }
    }

    

}
