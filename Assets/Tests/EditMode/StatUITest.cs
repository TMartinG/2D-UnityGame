using TMPro;
using UnityEngine;
using NUnit.Framework;
using System.Reflection;

public class StatUITest
{
     private GameObject obj;
    private YellowStatUpdate yellowUI;
    private RedStatUpdate redUI;

    private Light_Yellow yellowLight;
    private Light_Red redLight;

    [SetUp]
    public void Setup()
    {
        obj = new GameObject();
        yellowUI = obj.AddComponent<YellowStatUpdate>();

        yellowLight = new GameObject().AddComponent<Light_Yellow>();
        yellowLight.enabled = false;

        yellowUI.explosionText = CreateTMP();
        yellowUI.doubleText = CreateTMP();
        yellowUI.bigText = CreateTMP();
        yellowUI.damageText = CreateTMP();
        yellowUI.fireRateText = CreateTMP();
        yellowUI.energyCostText = CreateTMP();
        yellowUI.speedText = CreateTMP();

        typeof(YellowStatUpdate)
            .GetField("yellowLight", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(yellowUI, yellowLight);

        redUI = obj.AddComponent<RedStatUpdate>();

        redLight = new GameObject().AddComponent<Light_Red>();
        redLight.enabled = false;

        redUI.iceText = CreateTMP();
        redUI.fireText = CreateTMP();
        redUI.movementText = CreateTMP();
        redUI.damageText = CreateTMP();
        redUI.intensityText = CreateTMP();
        redUI.energyCostText = CreateTMP();

        typeof(RedStatUpdate)
            .GetField("redLight", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(redUI, redLight);
    }

    private TextMeshProUGUI CreateTMP()
    {
        var go = new GameObject();
        return go.AddComponent<TextMeshProUGUI>();
    }


    [Test]
    public void Yellow_ExplosionEnabled_ShowsText()
    {
        yellowLight.explosionEnabled = true;

        yellowUI.UpdateStats();

        Assert.IsTrue(yellowUI.explosionText.enabled);
    }

    [Test]
    public void Yellow_ExplosionDisabled_HidesText()
    {
        yellowLight.explosionEnabled = false;

        yellowUI.UpdateStats();

        Assert.IsFalse(yellowUI.explosionText.enabled);
    }

    [Test]
    public void Yellow_Stats_AreSetCorrectly()
    {
        yellowLight.baseDamage = 10;
        yellowLight.baseFireRate = 2;
        yellowLight.baseEnergyCost = 5;
        yellowLight.baseSpeed = 7;

        yellowUI.UpdateStats();

        Assert.AreEqual("10", yellowUI.damageText.text);
        Assert.AreEqual("2", yellowUI.fireRateText.text);
        Assert.AreEqual("5", yellowUI.energyCostText.text);
        Assert.AreEqual("7", yellowUI.speedText.text);
    }


    [Test]
    public void Red_FireEnabled_SetsEnabled()
    {
        redLight.fireEnabled = true;

        redUI.UpdateStats();

        Assert.IsTrue(redUI.fireText.enabled);
    }

    [Test]
    public void Red_FireUpgrade_SetsRedColor()
    {
        redLight.fireEnabled = true;
        redLight.upgradedFire = true;

        redUI.UpdateStats();

        Assert.AreEqual(Color.red, redUI.fireText.color);
    }

    [Test]
    public void Red_IceUpgrade_SetsCyanColor()
    {
        redLight.iceEnabled = true;
        redLight.upgradedIce = true;

        redUI.UpdateStats();

        Assert.AreEqual(Color.cyan, redUI.iceText.color);
    }

    [Test]
    public void Red_MovementEnabled_ShowsText()
    {
        redLight.betterMovementEnabled = true;

        redUI.UpdateStats();

        Assert.IsTrue(redUI.movementText.enabled);
    }

    [Test]
    public void Red_Stats_AreUpdatedCorrectly()
    {
        redLight.baseDamage = 20;
        redLight.baseFireRate = 3;
        redLight.baseEnergyCost = 8;

        redUI.UpdateStats();

        Assert.AreEqual("20", redUI.damageText.text);
        Assert.AreEqual("3", redUI.intensityText.text);
        Assert.AreEqual("8", redUI.energyCostText.text);
    }
}
