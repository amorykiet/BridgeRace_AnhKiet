using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState<Bot>
{
    public override void OnEnter(Bot owner)
    {

    }

    public override void OnExcute(Bot owner)
    {
        // neu ma vuot so luong gach can thiet de di xay cau
        // thi se xay cau
        if (owner.IsEnoughBrickToBuild())
        {
            // di xay cau

        }
        else
        {
            // nhat gach tiep
        }
    }

    public override void OnExit(Bot owner)
    {

    }
}
