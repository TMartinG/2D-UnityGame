using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyStatemachineTest
{
   private EnemyStateMachine stateMachine;

    [SetUp]
    public void Setup()
    {
        stateMachine = new EnemyStateMachine();
    }

    [Test]
    public void EntersStateOnStart()
    {
        var state = new FakeEnemyState();

        stateMachine.Initialize(state);

        Assert.AreEqual(state, stateMachine.currentState);
        Assert.IsTrue(state.enterCalled);
    }

    [Test]
    public void ChangeExitEnterOnNewState()
    {
        var oldState = new FakeEnemyState();
        var newState = new FakeEnemyState();

        stateMachine.Initialize(oldState);
        stateMachine.ChangeState(newState);

        Assert.IsTrue(oldState.exitCalled);
        Assert.IsTrue(newState.enterCalled);
        Assert.AreEqual(newState, stateMachine.currentState);
    }

    [Test]
    public void UpdateCallsCurrentStateUpdate()
    {
        var state = new FakeEnemyState();
        stateMachine.Initialize(state);

        stateMachine.Update();

        Assert.IsTrue(state.updateCalled);
    }

    [Test]
    public void FixedUpdateCallsCurrentStateFixedUpdate()
    {
        var state = new FakeEnemyState();
        stateMachine.Initialize(state);

        stateMachine.FixedUpdate();

        Assert.IsTrue(state.fixedUpdateCalled);
    }

    [Test]
    public void UpdateWithNullStateDoesNotThrow()
    {
        Assert.DoesNotThrow(() => stateMachine.Update());
    }

    [Test]
    public void FixedUpdateWithNullStateDoesNotThrow()
    {
        Assert.DoesNotThrow(() => stateMachine.FixedUpdate());
    }
}

public class FakeEnemyState : IEnemyState
{
    public bool enterCalled;
    public bool exitCalled;
    public bool updateCalled;
    public bool fixedUpdateCalled;

    public void Enter() => enterCalled = true;
    public void Exit() => exitCalled = true;
    public void Update() => updateCalled = true;
    public void FixedUpdate() => fixedUpdateCalled = true;
}