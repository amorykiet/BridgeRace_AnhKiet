using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Stage> stageList = new();
    [SerializeField] private CameraFollow cam;
    [SerializeField] private Transform winPos;

    private int currentStage = 0;

    private void Start()
    {
        OnInit();
    }

    private void OnEnable()
    {
        Player.onPlayerOpenDoor += OnOpenDoor;
        Player.onPlayerWin += OnWin;
        Bot.onBotOpenDoor += OnOpenDoor;
        Bot.onBotWin += OnWin;
    }


    public Stage GetStage(int stage)
    {
        return stageList[stage];
    }

    private void OnWin()
    {
        cam.FollowToTarget(winPos);
    }

    private void OnOpenDoor(int currentStage)
    {
        if (this.currentStage >= currentStage)
        {
            return;
        }
        this.currentStage = currentStage;
        stageList[currentStage].SpawnBrick();
    }

    public void OnInit()
    {
        cam = FindAnyObjectByType<CameraFollow>();
        stageList[currentStage].SpawnBrick();
    }

}
