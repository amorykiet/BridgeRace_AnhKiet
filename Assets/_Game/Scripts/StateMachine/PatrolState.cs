using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : BaseState<Bot>
{

    public override void OnEnter(Bot owner)
    {
        owner.ChangeAnim("run");
        owner.agent.SetDestination(owner.GetStage().GetRandomBrickPosition());
    }

    public override void OnExcute(Bot owner)
    {
        if(owner.BrickCollected >= owner.GetStage().MaxBricksPerColor)
        {
            owner.ChangeState(new BuildState());
        }

        if(owner.agent.pathStatus == NavMeshPathStatus.PathComplete && owner.agent.remainingDistance == 0)
        {
            if(Random.Range(0,5) != 0)
            {
                owner.ChangeState(new IdleState());
            }
            else
            {
                owner.ChangeState(new BuildState());
            }
        }
    }

    public override void OnExit(Bot owner)
    {

    }
}
