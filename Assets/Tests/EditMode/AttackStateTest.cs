using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class AttackStateTest : TestBase
{
   private GameObject enemyGO;
    private GameObject playerGO;

    private TestEnemy enemy;
    private AttackState attackState;

    private FakeVision vision;
    private FakeAttack attack;

    [SetUp]
    public void Setup()
    {
        enemyGO = new GameObject("Enemy");
        playerGO = new GameObject("Player");

        enemy = enemyGO.AddComponent<TestEnemy>();
        enemy.animator = enemyGO.AddComponent<Animator>();

        enemy.player = playerGO.transform;
        enemy.stateMachine = new EnemyStateMachine();

        vision = enemyGO.AddComponent<FakeVision>();
        attack = new FakeAttack();

        enemy.vision = vision;
        enemy.attack = attack;

        enemy.searchState = new SearchState(enemy);
        enemy.chaseState = new ChaseState(enemy);

        enemy.attackAttackRange = 5f;
        enemy.keepDistance = 0f;
        enemy.chaseLostSightDelay = 2f;
        enemy.enemy_Type = EnemyType.basic;

        attackState = new AttackState(enemy);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(enemyGO);
        Object.DestroyImmediate(playerGO);
    }

    [Test]
    public void AttackOnEner()
    {
        attackState.Enter();

        Assert.AreEqual(EnemyState.attacking, enemy.enemy_State);
    }

    [Test]
    public void UpdateWhenPlayerVisible()
    {
        vision.canSee = true;
        playerGO.transform.position = new Vector3(2f, 0f, 0f);

        attackState.Update();

        Assert.IsTrue(attack.attackCalled);
        Assert.AreEqual(playerGO.transform, attack.lastTarget);
    }

    [Test]
    public void UpdatesLastSeenPosition()
    {
        vision.canSee = true;
        playerGO.transform.position = new Vector3(3f, 1f, 0f);

        attackState.Update();

        Assert.AreEqual(playerGO.transform.position, (Vector3)enemy.lastSeenPosition);
        Assert.IsTrue(enemy.hasLastSeenPosition);
        Assert.AreEqual(0f, enemy.attackLostSightTimer);
    }

    [Test]
    public void ChangesFromAttackToChaseState()
    {
        vision.canSee = true;

        enemy.transform.position = Vector3.zero;
        playerGO.transform.position = new Vector3(10f, 0f, 0f);

        enemy.stateMachine.Initialize(attackState);

        attackState.Update();

        Assert.AreEqual(enemy.chaseState, enemy.stateMachine.currentState);
    }
}
public class FakeAttack : IAttack
{
    public bool attackCalled;
    public Transform lastTarget;
    public int attackCount;

    public void Attack(Transform target)
    {
        attackCalled = true;
        lastTarget = target;
        attackCount++;
    }

    public void Reset()
    {
        attackCalled = false;
        lastTarget = null;
        attackCount = 0;
    }
}