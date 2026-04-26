using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public IEnemyState currentState;

    public void Initialize(IEnemyState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }
}
