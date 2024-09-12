using Scriptable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class Bot : Character
{
    //In Bot
    public NavMeshAgent agent;
    [SerializeField] private Level currentLevel;

    public Level CurrentLevel { 
        get { return currentLevel; } 
        set { currentLevel = value; }
    }

    private BaseState<Bot> currentState;
    private float timeToBuild;


    private void Update()
    {
        if (stopMovement) return;
        if (currentState == null) return;
        currentState.OnExcute(this);
    }

    override protected void CollideDoor(Collider other)
    {
        Door door = other.GetComponent<Door>();
        if (currentStageIndex < door.stageToOpenIndex)
        {
            currentStageIndex = door.stageToOpenIndex;
            base.OnOpenDoor(this, currentStageIndex, myColor);
            ChangeState(new PatrolState());
        }
    }

    override protected void CollideWinPos()
    {
        base.OnWin(this);
    }

    override public void OnInit(ColorType colorType)
    {
        myColor = colorType;
        skinnedMeshRenderer.material = colorData.GetMat(myColor);
        currentStageIndex = 0;
        stopMovement = false;

        ChangeState(new IdleState());
    }

    public void ChangeState(BaseState<Bot> newState)
    {
        if (currentState != null) currentState.OnExit(this);
        currentState = newState;
        currentState.OnEnter(this);
    }

    public void StandOnBrickOnBridge(RaycastHit hitBrickOnStair)
    {
        BrickOnStair brick = hitBrickOnStair.collider.GetComponent<BrickOnStair>();

        if (!brick.IsSameColor(myColor))
        {
            if (BrickCollected > 0)
            {
                brick.ChangeColor(myColor);
                RemoveBrick();
            }
            else
            {
                ChangeState(new PatrolState());
            }
        }


    }

    public Stage GetStage()
    {
        return currentLevel.GetStage(currentStageIndex);
    }

    override public void Stop()
    {
        stopMovement = true;
        agent.enabled = false;
    }

    
}
