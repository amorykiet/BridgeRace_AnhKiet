using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Stage> stageList = new();

    private int currentStage = 0;

    private void Start()
    {
        OnInit();
    }

    private void OnEnable()
    {
        Player.onPlayerOpenDoor += OnPlayerOpenDoor;
    }

    private void OnPlayerOpenDoor(int currentStage)
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
        stageList[currentStage].SpawnBrick();
    }


}
