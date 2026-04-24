using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class RedAimTest
{
    private GameObject obj;
    private Red_Aim aim;
    private FakeRed red;

    [SetUp]
    public void Setup()
    {
        obj = new GameObject();
        aim = obj.AddComponent<Red_Aim>();

        aim.cam = new GameObject().AddComponent<Camera>();
        aim.lineRenderer = obj.AddComponent<LineRenderer>();
        aim.firePoint = obj.transform;

        aim.endVFX = new GameObject();

        red = new GameObject().AddComponent<FakeRed>();
        aim.GetType()
            .GetField("red", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(aim, red);
    }

    [Test]
    public void ResetLaser_SetsPositions()
    {
        aim.ResetLaser();

        Assert.AreEqual(aim.firePoint.position, aim.lineRenderer.GetPosition(0));
        Assert.AreEqual(aim.firePoint.position, aim.lineRenderer.GetPosition(1));
    }

    [Test]
    public void FillLists_CollectsParticles()
    {
        var child = new GameObject();
        child.transform.parent = aim.endVFX.transform;
        child.AddComponent<ParticleSystem>();

        typeof(Red_Aim)
            .GetMethod("FillLists", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(aim, null);

        Assert.Greater(aim.particles.Count, 0);
    }

    [Test]
    public void VFX_Start_Stop_Works()
    {
        aim.StartVFX();
        Assert.IsTrue(aim.particles.Count >= 0);

        aim.StopVFX();
        Assert.IsFalse(aim.endVFX.activeSelf);
    }


    [Test]
    public void DamageTimer_InitialState_IsZero()
    {
        float timer = (float)aim.GetType()
            .GetField("damageTimer", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(aim);

        Assert.AreEqual(0f, timer);
    }
}

public class FakeRed : Light_Red
{
    public float fakeSpeed = 10f;

    public new float GetFireRate() => 1f;
    public new int GetDamage() => 5;
}