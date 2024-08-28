using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    private BaseState<Bot> currentState;

    private void Start()
    {
        currentState = new IdleState();
    }

    private void Update()
    {
        if (currentState == null) return;
        currentState.OnExcute(this);
    }

    public void ChangeState(BaseState<Bot> newState)
    {
        if (currentState != null) currentState.OnExit(this);
        currentState = newState;
        currentState.OnEnter(this);
    }

    public bool IsEnoughBrickToBuild()
    {
        return true;
    }

}
