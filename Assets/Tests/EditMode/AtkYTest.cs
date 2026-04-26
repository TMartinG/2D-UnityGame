using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class AtkYTest
{
    private GameObject obj;
    private Atk_Y projectile;
    private FakeYellow shooter;

    [SetUp]
    public void Setup()
    {
        obj = new GameObject();
        projectile = obj.AddComponent<Atk_Y>();

        var rb = obj.AddComponent<Rigidbody2D>();

        shooter = new GameObject().AddComponent<FakeYellow>();
        projectile.Init(shooter);
    }

    [Test]
    public void InitSetsShooter()
    {
        Assert.NotNull(GetPrivate(projectile, "light_Yellow"));
    }

    [Test]
    public void ATKHasDamage()
    {
        Assert.AreEqual(2, shooter.GetDamage());
    }

    [Test]
    public void ATKHasSpeed()
    {
        Assert.AreEqual(10f, shooter.GetSpeed());
    }

    private object GetPrivate(object obj, string field)
    {
        return obj.GetType()
            .GetField(field, BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(obj);
    }

}
public class FakeYellow : Light_Yellow
{
    public new float GetSpeed() => 10f;
    public new int GetDamage() => 2;
}
public class FakeEnemy : MonoBehaviour
{
    public bool damaged;

    public void GetDamaged(short dmg, Vector3 pos)
    {
        damaged = true;
    }
}
public class FakeExplosion : MonoBehaviour { }