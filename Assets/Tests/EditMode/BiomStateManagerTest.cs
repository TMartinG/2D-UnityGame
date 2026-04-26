using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BiomStateManagerTest : TestBase
{
    private BiomStateManagger CreateManager(int size = 3)
    {
        var go = new GameObject("BiomManager");
        var manager = go.AddComponent<BiomStateManagger>();

        manager.biomes = new GameObject[size];
        manager.biomesPrefabs = new GameObject[size];

        for (int i = 0; i < size; i++)
        {
            manager.biomesPrefabs[i] = new GameObject($"Prefab_{i}");
        }

        manager.Awake();

        return manager;
    }

    [Test]
    public void BiomsCreatedAndDeactivated()
    {
        var manager = CreateManager(3);

        Assert.AreEqual(3, manager.biomes.Length);

        foreach (var biome in manager.biomes)
        {
            Assert.IsNotNull(biome);
            Assert.IsFalse(biome.activeSelf);
        }
    }

    [Test]
    public void ActivateCorrectBiome()
    {
        var manager = CreateManager(3);

        manager.SetBiome(2); // index-1 => 1

        Assert.IsFalse(manager.biomes[0].activeSelf);
        Assert.IsTrue(manager.biomes[1].activeSelf);
        Assert.IsFalse(manager.biomes[2].activeSelf);
    }

    [Test]
    public void SetCurrentBiomeCorrectly()
    {
        var manager = CreateManager(2);

        manager.SetCurrentBiom(1);

        Assert.AreEqual(manager.biomes[0], manager.currentBiome);
    }

    [Test]
    public void ApplyWorldStateToBiome()
    {
        var manager = CreateManager(1);

        manager.SetCurrentBiom(1);

        var enemyGO = new GameObject();
        enemyGO.transform.parent = manager.currentBiome.transform;
        enemyGO.AddComponent<FakeEnemy>();

        manager.ApplyWorldStateToBiome();

        Assert.Pass("No exceptions thrown");
    }
}

