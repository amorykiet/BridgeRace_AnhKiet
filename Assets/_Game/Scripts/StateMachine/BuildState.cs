using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : BaseState<Bot>
{
    RaycastHit hit;

    public override void OnEnter(Bot owner)
    {
        owner.ChangeAnim("run");
        owner.agent.SetDestination(owner.winPos.position);
    }

    public override void OnExcute(Bot owner)
    {
        if (Physics.Raycast(owner.TF.position, Vector3.forward, out hit, 0.5f, owner.brickOnStairMask))
        {
            owner.StandOnBrickOnBridge(hit);
        }

    }

    public override void OnExit(Bot owner)
    {
    }
}
