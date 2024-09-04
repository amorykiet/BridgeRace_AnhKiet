using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState<Bot>
{
    private const float MaxTimeToPatrol = 2;
    private const float MinTimeToPatrol = 0;

    private float timeToPatrol;
    private float timeCount;

    public override void OnEnter(Bot owner)
    {
        Debug.Log("On Idle");
        owner.ChangeAnim("idle");
        timeToPatrol = Random.Range(MinTimeToPatrol, MaxTimeToPatrol);
        timeCount = 0;
    }

    public override void OnExcute(Bot owner)
    {
        timeCount += Time.deltaTime;
        if (timeCount > timeToPatrol)
        {
            owner.ChangeState(new PatrolState());
        } 
    }

    public override void OnExit(Bot owner)
    {
        
    }
}
