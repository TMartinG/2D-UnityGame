using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PatrolStateTest : TestBase
{
   private GameObject enemyObj;
    private TestEnemy enemy;
    private PatrolState patrolState;
    private FakeMovement movement;
    private FakeVision vision;

    [SetUp]
    public void Setup()
    {
        enemyObj = new GameObject();

        enemy = enemyObj.AddComponent<TestEnemy>();
        enemy.animator = enemyObj.AddComponent<Animator>();

        movement = new FakeMovement();
        vision = enemyObj.AddComponent<FakeVision>();

        enemy.movement = movement;
        enemy.vision = vision;
        enemy.patrolRadius = 5f;
        enemy.patrolChangeInterval = 2f;

        enemy.player = new GameObject().transform;

        enemy.stateMachine = new EnemyStateMachine();
        enemy.chaseState = new ChaseState(enemy);

        patrolState = new PatrolState(enemy);
    }

    [Test]
    public void Enter_SetsEnemyState()
    {
        patrolState.Enter();

        Assert.AreEqual(EnemyState.patrolling, enemy.enemy_State);
    }

    [Test]
    public void FixedUpdate_CallsMoveTo()
    {
        patrolState.Enter();

        patrolState.FixedUpdate();

        Assert.IsTrue(movement.moveToCalled);
    }

    [Test]
    public void Update_WhenPlayerVisible_ChangesState()
    {
        vision.canSee = true;

        enemy.stateMachine.Initialize(patrolState);

        patrolState.Update();

        Assert.AreEqual(enemy.chaseState, enemy.stateMachine.currentState);
    }

}
public class FakeMovement : IMovement
{
    public bool moveToCalled;
    public bool moveToSearchCalled;
    public Vector2 lastTarget;
    public Vector2 lastSearchTarget;

    public void MoveTo(Vector2 targetPosition)
    {
        moveToCalled = true;
        lastTarget = targetPosition;
    }

    public void MoveTo_Search(Vector2 targetPosition)
    {
        moveToSearchCalled = true;
        lastSearchTarget = targetPosition;
    }

    public void Reset()
    {
        moveToCalled = false;
        moveToSearchCalled = false;
        lastTarget = Vector2.zero;
        lastSearchTarget = Vector2.zero;
    }
}

public class FakeVision : EnemyVision
{
    public bool canSee;

    public new bool CanSeeTarget(Transform target)
    {
        return canSee;
    }
}

public class FakeState : IEnemyState
{
    public bool enterCalled;
    public bool exitCalled;

    public void Enter() => enterCalled = true;
    public void Exit() => exitCalled = true;
    public void Update() { }
    public void FixedUpdate() { }
}