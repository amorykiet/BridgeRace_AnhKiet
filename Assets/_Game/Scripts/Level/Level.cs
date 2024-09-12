using Scriptable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Dictionary<ColorType, Vector3> characterPosDictionary = new();
    public List<Transform> winPosList = new();
    public Transform winPos;

    [SerializeField] private List<Stage> stageList;
    [SerializeField] private CameraFollow cam;
    [SerializeField] private Transform startPos;
    [SerializeField] private ColorData colorData;
    [SerializeField] private int colorNumber = 4;



    private void OnEnable()
    {
        Character.onOpenDoor += OnOpenDoor;
        Character.onWin += FollowWinPos;
    }

    private void OnDisable()
    {
        Character.onOpenDoor -= OnOpenDoor;
        Character.onWin -= FollowWinPos;

    }

    private void FollowWinPos(Character character)
    {
        cam.FollowToTarget(winPos);
    }

    private void OnOpenDoor(Character character, int currentStage, ColorType color)
    {
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

    public Stage GetStage(int stage)
    {
        return stageList[stage];
    }

}
