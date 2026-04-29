using TMPro;
using UnityEngine;
using NUnit.Framework;
using System.Reflection;
public class SkillModeTest
{
    private GameObject obj;
    private SkillModes modes;

    [SetUp]
    public void Setup()
    {
        Random.InitState(12345);

        obj = new GameObject();
        modes = obj.AddComponent<SkillModes>();

        modes.modes = new GameObject[]
        {
            new GameObject("A"),
            new GameObject("B"),
            new GameObject("C"),
            new GameObject("D"),
            new GameObject("E"),
            new GameObject("F"),
            new GameObject("Boss1"),
            new GameObject("Boss2")
        };

        modes.chunkSpawnCounts = new int[] { 1, 2, 2, 1 };
    }

    [Test]
    public void CorrectChunkCount()
    {
        InvokeSetup();

        Assert.AreEqual(modes.chunkSpawnCounts.Length, modes.chunkSkills.Count);
    }

    

    [Test]
    public void SetupSkillsDoesNotLoseModes()
    {
        InvokeSetup();

        int assigned = 0;

        foreach (var chunk in modes.chunkSkills)
        {
            assigned += chunk.Count;
        }

        Assert.LessOrEqual(assigned, modes.modes.Length - 2);
    }


    [Test]
    public void SetupSkillsRespectsSpawnCounts()
    {
        InvokeSetup();

        for (int i = 0; i < modes.chunkSpawnCounts.Length; i++)
        {
            Assert.LessOrEqual(modes.chunkSkills[i].Count, modes.chunkSpawnCounts[i]);
        }
    }

    private void InvokeSetup()
    {
        MethodInfo setup = typeof(SkillModes)
            .GetMethod("SetupSkills", BindingFlags.NonPublic | BindingFlags.Instance);

        setup.Invoke(modes, null);
    }
}
