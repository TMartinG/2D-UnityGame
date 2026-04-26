using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class ChaseStateTest : TestBase
{
    private GameObject enemyGO;
    private GameObject playerGO;

    private TestEnemy enemy;
    private ChaseState chaseState;

    private FakeMovement movement;
    private FakeVision vision;

    [SetUp]
    public void Setup()
    {
        enemyGO = new GameObject("Enemy");
        playerGO = new GameObject("Player");

        enemy = enemyGO.AddComponent<TestEnemy>();
        enemy.animator = enemyGO.AddComponent<Animator>();

        enemy.player = playerGO.transform;
        enemy.stateMachine = new EnemyStateMachine();

        movement = new FakeMovement();
        vision = enemyGO.AddComponent<FakeVision>();

        enemy.movement = movement;
        enemy.vision = vision;

        enemy.attackState = new AttackState(enemy);
        enemy.searchState = new SearchState(enemy);

        enemy.chaseAttackRange = 5f;
        enemy.chaseLostSightDelay = 2f;

        chaseState = new ChaseState(enemy);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(enemyGO);
        Object.DestroyImmediate(playerGO);
    }

    [Test]
    public void ChaseOnStart()
    {
        chaseState.Enter();

        Assert.AreEqual(EnemyState.chasing, enemy.enemy_State);
    }

    [Test]
    public void UpdateWhenPlayerVisibleAndStoresLastSeenPosition()
    {
        vision.canSee = true;
        playerGO.transform.position = new Vector3(5f, 2f, 0f);

        chaseState.Update();

        Assert.AreEqual(playerGO.transform.position, (Vector3)enemy.lastSeenPosition);
        Assert.IsTrue(enemy.hasLastSeenPosition);
        Assert.AreEqual(0f, enemy.chaseLostSightTimer);
    }

    [Test]
    public void ChangesFromChaseToAttackState()
    {
        vision.canSee = true;

        enemy.transform.position = Vector3.zero;
        playerGO.transform.position = new Vector3(1f, 0f, 0f);

        enemy.stateMachine.Initialize(chaseState);

        chaseState.Update();

        Assert.AreEqual(enemy.attackState, enemy.stateMachine.currentState);
    }

    [Test]
    public void BasicEnemyUsesMoveTo()
    {
        enemy.enemy_Type = EnemyType.basic;
        playerGO.transform.position = new Vector3(3f, 0f, 0f);

        chaseState.FixedUpdate();

        Assert.IsTrue(movement.moveToCalled);
        Assert.AreEqual((Vector2)playerGO.transform.position, movement.lastTarget);
    }

    [Test]
    public void FlyingEnemyUsesMoveToSearch()
    {
        enemy.enemy_Type = EnemyType.flying;
        playerGO.transform.position = new Vector3(4f, 1f, 0f);

        chaseState.FixedUpdate();

        Assert.IsTrue(movement.moveToSearchCalled);
        Assert.AreEqual((Vector2)playerGO.transform.position, movement.lastSearchTarget);
    }
}
