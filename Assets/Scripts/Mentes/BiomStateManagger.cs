using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomStateManagger : MonoBehaviour
{
    public static BiomStateManagger Instance;

    public GameObject[] biomes;
    public GameObject[] biomesPrefabs;

    public GameObject currentBiome;

    public void Awake()
    {
        for (int i = 0; i < biomes.Length; i++)
        {
            biomes[i] =  Instantiate(biomesPrefabs[i]);
            biomes[i].SetActive(false);
        }
        //biomes[0].SetActive(true);

        Instance = this;

    }

    public void SetBiome(int index)
    {
        for (int i = 0; i < biomes.Length; i++)
        {
            biomes[i].SetActive(i == index - 1);
        }
    }

    public void SetCurrentBiom(int index)
    {
        currentBiome = biomes[index-1];
    }

    public void ResetBiome(int index)
    {
        StartCoroutine(ResetBiomeRoutine(index));
    }

    IEnumerator ResetBiomeRoutine(int index)
    {
        Vector3 pos = currentBiome.transform.position;
        
        if (currentBiome != null)
        {
            //Debug.Log("Destroying: " + currentBiome.name);
            currentBiome.SetActive(false);
            Destroy(currentBiome.gameObject);
            //Debug.Log("current biom: " + currentBiome.name);
        }

        yield return null;


        currentBiome = Instantiate(biomesPrefabs[index - 1], pos, Quaternion.identity);
        biomes[index-1] = currentBiome;
        currentBiome.SetActive(true);
    }

    public void ApplyWorldStateToBiome()
    {
        var enemies = currentBiome.GetComponentsInChildren<Enemy_Base>(true);

        foreach (var enemy in enemies)
        {
            enemy.ApplyState(enemy.enemy_HP);
        }
    }
}
