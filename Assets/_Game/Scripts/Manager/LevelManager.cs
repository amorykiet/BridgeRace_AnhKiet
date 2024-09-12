using Scriptable;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public FixedJoystick joystick;

    [SerializeField] private List<Level> levelList = new();
    [SerializeField] private Player player;
    [SerializeField] private Bot bot;
    [SerializeField] private CameraFollow cam;

    private List<Level> runtimeLevelList = new();
    private List<Character> characterList = new();

    private int currentLevel;

    
    private void OnEnable()
    {
        Character.onWin += CompleteLevel;
    }

    private void OnDisable()
    {
        Character.onWin -= CompleteLevel;
    }

    private void StopCharacterMove()
    {
        foreach (Character character in characterList)
        {
            character.Stop();
        }

    }

    private void CompleteLevel(Character character)
    {
        StopCharacterMove();
        List<Character> sortedCharacterList = characterList.OrderBy(o => o.BrickCollected).ToList();
        ClearCharacterBrick();

        //character win pos
        character.GoToPos(levelList[currentLevel].winPos);
        character.Dance();
        sortedCharacterList.Remove(character);

        //other character drop the one with the least bricks
        for (int i = 1; i < 3; i++)
        {
            sortedCharacterList[i].GoToPos(levelList[currentLevel].winPosList[i - 1]);
            sortedCharacterList[i].Dance();

        }

        GameManager.ChangeState(GameState.Finish);

        if (character is Player)
        {
            WinLevel();
        }
        else if (character is Bot)
        {
            FailLevel();
        }
    }

    private void ClearCharacterBrick()
    {
        foreach(Character character in characterList)
        {
            character.ClearBrick();
        }
    }

    private void WinLevel()
    {
        if (currentLevel < levelList.Count - 1)
        {
            PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
        }
        Invoke(nameof(ChangeCanvasToWin), 2);
    }

    private void FailLevel()
    {
        StopCharacterMove();
        Invoke(nameof(ChangeCanvasToLose), 2);
    }

    private void ChangeCanvasToWin()
    {
        UIManager.Ins.GetUI<CanvasGamePlay>().PlayerWin();
    }

    private void ChangeCanvasToLose()
    {
        UIManager.Ins.GetUI<CanvasGamePlay>().PlayerLose();
    }

    public void OnInit()
    {
        if (PlayerPrefs.HasKey("currentLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("currentLevel");
        }
        else
        {
            currentLevel = 0;
        }

        LoadLevel(currentLevel);
    }

    public void ReloadLevel()
    {
        ClearLevel();
        LoadLevel(currentLevel);
    }

    public void LoadLevel(int levelIndex)
    {
        //Setup Level
        Level level_ = Instantiate(levelList[levelIndex], transform);
        level_.OnInit();
        runtimeLevelList.Add(level_);
        Dictionary<ColorType, Vector3> characterPos = level_.characterPosDictionary;

        //Setup Player
        Player player_ = Instantiate(player);
        player_.joyStick = joystick;
        characterList.Add(player_);
        cam.FollowToTarget(player_.transform);

        //Setup Bot
        for (int i = 0; i < 3; i++)
        {
            Bot bot_ = Instantiate(bot);
            bot_.CurrentLevel = level_;
            characterList.Add(bot_);
        }

        //Setup for character
        ColorType randomColor;
        foreach (Character character in characterList)
        {
            randomColor = characterPos.ElementAt(Random.Range(0, characterPos.Count)).Key;
            character.winPos = level_.winPos;
            character.OnInit(randomColor);
            character.transform.position = characterPos[randomColor];
            characterPos.Remove(randomColor);
        }

    }

    public void LoadNextLevel()
    {
        ClearLevel();
        if (currentLevel == levelList.Count - 1)
        {
            LoadLevel(currentLevel);
            return;
        }
        LoadLevel(++currentLevel);
    }

    public void ClearLevel()
    {
        cam.FollowToTarget(cam.transform);
        SimplePool.CollectAll();

        foreach (var level in runtimeLevelList)
        {
            Destroy(level.gameObject);
        }

        foreach (var character in characterList)
        {
            Destroy(character.gameObject);
        }

        runtimeLevelList.Clear();
        characterList.Clear();
    }


}
