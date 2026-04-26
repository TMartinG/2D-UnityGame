using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class PlayerCharacterTest : TestBase
{
    private Player_Character player;

    [SetUp]
    public void Setup()
    {
         var obj = new GameObject();
        player = obj.AddComponent<Player_Character>();

        player.enabled = false;

        player.GetType().GetField("heartsSymbol", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(player, CreateHearts(5));

        player.GetType().GetField("yellowEnergyOrb", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(player, new GameObject().transform);

        player.GetType().GetField("redEnergyOrb", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(player, new GameObject().transform);

        player.playerCurrentHP = 5;

        typeof(Player_Character)
            .GetField("playerMaxHP", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(player, 5);

        player.GetType().GetField("headSpriteRenderer", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(player, new GameObject().AddComponent<SpriteRenderer>());

        player.GetType().GetField("bodySpriteRenderer", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(player, new GameObject().AddComponent<SpriteRenderer>());
    }

    GameObject[] CreateHearts(int count)
    {
        GameObject[] hearts = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            var go = new GameObject();
            go.AddComponent<Image>();
            hearts[i] = go;
        }
        return hearts;
    }

    [Test]
    public void HealIncreasesHP()
    {
        player.playerCurrentHP = 4;
        player.Heal(5);

        Assert.AreEqual(5, player.playerCurrentHP);
    }

    [Test]
    public void TakeDamageDecreasesHP()
    {
        player.playerCurrentHP = 5;

        player.TakeDamage(2);

        Assert.AreEqual(3, player.playerCurrentHP);
    }

    [Test]
    public void InvincibleWorks()
    {
        player.playerCurrentHP = 5;
        player.SetInvincible(true);

        player.TakeDamage(3);

        Assert.AreEqual(5, player.playerCurrentHP);
    }

    [Test]
    public void YellowEnergyDecreasesIfCan()
    {
        player.playerYellowCurrentEnergy = 50f;

        bool result = player.UseYellowEnergy(20f);

        Assert.IsTrue(result);
        Assert.AreEqual(30f, player.playerYellowCurrentEnergy);
    }

    [Test]
    public void YellowEnergyDrainWorksCorrectly()
    {
        player.playerYellowCurrentEnergy = 10f;

        bool result = player.UseYellowEnergy(20f);

        Assert.IsFalse(result);
        Assert.AreEqual(10f, player.playerYellowCurrentEnergy);
    }

    [Test]
    public void AddRedEnergyWorks()
    {
        player.playerRedCurrentEnergy = 90f;
        player.playerRedMaxEnergy = 100f;

        player.AddRedEnergy(50f);

        Assert.AreEqual(100f, player.playerRedCurrentEnergy);
    }

    [Test]
    public void YellowEnergyUpdatesScale()
    {
        player.playerYellowMaxEnergy = 100f;
        player.playerYellowCurrentEnergy = 50f;

        player.AddYellowEnergy(0);

        Transform orb = (Transform)player.GetType()
            .GetField("yellowEnergyOrb", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(player);

        Assert.Greater(orb.localScale.x, 0);
    }

    [Test]
    public void CanEnd_ReturnsTrue_WhenBothKeys()
    {
        Assert.IsFalse(player.CanEnd());
    }
}

