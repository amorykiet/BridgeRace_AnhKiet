using Scriptable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform winPos;

    [SerializeField] private List<Stage> stageList;
    [SerializeField] private CameraFollow cam;
    [SerializeField] private Transform startPos;
    [SerializeField] private ColorData colorData;
    [SerializeField] private int colorNumber = 4;


    public Dictionary<ColorType, Vector3> characterPosDictionary = new();

    private void OnEnable()
    {
        Player.onPlayerOpenDoor += OnOpenDoor;
        Player.onPlayerWin += OnFinish;
        Bot.onBotOpenDoor += OnOpenDoor;
        Bot.onBotWin += OnFinish;
    }

    private void OnDisable()
    {
        Player.onPlayerOpenDoor -= OnOpenDoor;
        Player.onPlayerWin -= OnFinish;
        Bot.onBotOpenDoor -= OnOpenDoor;
        Bot.onBotWin -= OnFinish;

    }

    public Stage GetStage(int stage)
    {
        return stageList[stage];
    }

    private void OnFinish()
    {
        cam.FollowToTarget(winPos);
    }

    private void OnOpenDoor(int currentStage, ColorType color)
    {
        //if (this.currentStage > currentStage)
        //{
        //    return;
        //}
        //this.currentStage = currentStage;
        stageList[currentStage].SpawnBrickByColor(color);
    }

    public void OnInit()
    {
        cam = FindAnyObjectByType<CameraFollow>();
        stageList[0].SpawnBrick();

        for (int i = 0; i < colorNumber; i++)
        {
            characterPosDictionary.Add((ColorType)(i + 1), startPos.transform.position + Vector3.right * 4 * i);
        }

    }


}
