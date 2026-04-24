using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class EnemyBaseTest : TestBase
{
    [Test]
    public void GetDamaged_ReducesHP()
    {
        var world = new GameObject();
        world.AddComponent<WorldStateManager>();

        var obj = new GameObject();
        var enemy = obj.AddComponent<TestEnemy>();

        enemy.uniqueID = "test_enemy";
        enemy.enemy_HP = 10;

        enemy.GetDamaged(3);

        Assert.AreEqual(7, enemy.enemy_HP);

    }

    [Test]
    public void GetDamaged_DoublesWhenFrozen()
    {
        var obj = new GameObject();
        var enemy = obj.AddComponent<TestEnemy>();
        enemy.uniqueID = "test_enemy2";

        enemy.enemy_HP = 10;

        // private field hack
        typeof(Enemy_Base)
            .GetField("isFrozen", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(enemy, true);

        enemy.GetDamaged(2);

        Assert.AreEqual(6, enemy.enemy_HP); // 2 * 2 = 4 damage
    }

    [Test]
    public void AddFireBuildUp_TriggersBurning()
    {
        var obj = new GameObject();
        var enemy = obj.AddComponent<TestEnemy>();

        typeof(Enemy_Base)
            .GetField("fireBuildUpThreshold", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(enemy, 3f);

        enemy.AddFireBuildUp(5f);

        bool isBurning = (bool)typeof(Enemy_Base)
            .GetField("isBurning", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(enemy);

        Assert.IsTrue(isBurning);
    }

    [Test]
    public void AddFreezeBuildUp_TriggersFreeze()
    {
        var obj = new GameObject();
        var enemy = obj.AddComponent<TestEnemy>();

        typeof(Enemy_Base)
            .GetField("freezeThreshold", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(enemy, 5f);

        enemy.AddFreezeBuildUp(10f);

        bool isFrozen = (bool)typeof(Enemy_Base)
            .GetField("isFrozen", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(enemy);

        Assert.IsTrue(isFrozen);
    }

    [Test]
    public void DistanceToPlayer_ReturnsCorrectValue()
    {
        var obj = new GameObject();
        var enemy = obj.AddComponent<TestEnemy>();

        var player = new GameObject();
        player.transform.position = new Vector3(3, 4, 0);

        enemy.player = player.transform;
        enemy.transform.position = Vector3.zero;

        float dist = enemy.DistanceToPlayer();

        Assert.AreEqual(5f, dist, 0.01f); // 3-4-5 háromszög
    }

    [Test]
    public void ApplyDebuff_Fire_SetsValues()
    {
        var obj = new GameObject();
        var enemy = obj.AddComponent<TestEnemy>();

        enemy.ApplyDebuff(
            DebuffType.fire,
            duration: 2f,
            fireDamage: 5,
            maxFireDuration: 10f,
            slowLerpSpeed: 0,
            freezeThreshold: 0,
            freezeDuration: 0
        );

        short dmg = (short)typeof(Enemy_Base)
            .GetField("fireDamage", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(enemy);

        Assert.AreEqual(5, dmg);
    }
}
public class TestEnemy : Enemy_Base
{
    public void ApplyEffect(GameObject target, float amount) { }

    protected override void Start() { } // kikapcsoljuk a heavy initet
}









public static class TestBootstrap
{
    public static void Init()
    {
        CreateSingleton<WorldStateManager>();
        CreateSingleton<SkillManager>();
        //CreateSingleton<Enemy_Bullet>();
        //CreateSingleton<FlyingRangedAttack>();
        // ide jöhet minden singletonod
    }

    public static T CreateSingleton<T>() where T : MonoBehaviour
    {
        var obj = new GameObject(typeof(T).Name);
        var instance = obj.AddComponent<T>();

        // Instance field beállítása (field vagy property támogatás)
        var type = typeof(T);

        var field = type.GetField("Instance",
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        if (field != null)
        {
            field.SetValue(null, instance);
            return instance;
        }

        var prop = type.GetProperty("Instance",
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(null, instance);
            return instance;
        }

        Debug.LogWarning($"{type.Name} nem tartalmaz static Instance-t!");
        return instance;
    }

    public static void Cleanup()
    {
        foreach (var obj in GameObject.FindObjectsOfType<GameObject>())
        {
            Object.DestroyImmediate(obj);
        }
    }
}



public class TestBase
{
    [SetUp]
    public virtual void BaseSetup()
    {
        TestBootstrap.Cleanup();
        TestBootstrap.Init();
    }

    [TearDown]
    public virtual void BaseTearDown()
    {
        TestBootstrap.Cleanup();
    }
}