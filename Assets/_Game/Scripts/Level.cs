using Scriptable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform winPos;

    [SerializeField] private List<Stage> stageList = new();
    [SerializeField] private CameraFollow cam;
    [SerializeField] private Transform startPos;
    [SerializeField] private ColorData colorData;
    [SerializeField] private int colorNumber = 4;


    public Dictionary<ColorType, Vector3> characterPosDictionary = new();
    private int currentStage = 0;

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

        for (int i = 0; i < colorNumber; i++)
        {
            characterPosDictionary.Add((ColorType)(i + 1), startPos.transform.position + Vector3.right * 4 * i);
        }

    }


}
