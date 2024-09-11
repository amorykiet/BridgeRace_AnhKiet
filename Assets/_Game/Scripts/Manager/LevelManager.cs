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

    [SerializeField] private List<Level> Levels = new();
    [SerializeField] private Player player;
    [SerializeField] private Bot bot;
    [SerializeField] private CameraFollow cam;

    private List<Level> levelList = new();
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
        if (character is Player)
        {
            WinLevel();
        }
        else if (character is Bot)
        {
            FailLevel();
        }
    }

    private void WinLevel()
    {
        if (currentLevel < Levels.Count - 1)
        {
            PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
        }
        StopCharacterMove();
        Invoke(nameof(ChangeCanvasToWin), 2);
        GameManager.ChangeState(GameState.Finish);
    }

    private void FailLevel()
    {
        StopCharacterMove();
        Invoke(nameof(ChangeCanvasToLose), 2);
        GameManager.ChangeState(GameState.Finish);
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
        Level level_ = Instantiate(Levels[levelIndex], transform);
        level_.OnInit();
        levelList.Add(level_);
        Dictionary<ColorType, Vector3> characterPos = level_.characterPosDictionary;

        //Setup Player
        Player player_ = Instantiate(player);
        player_.joyStick = joystick;
        characterList.Add(player_);
        cam.FollowToTarget(player_.transform);
        //player_.winPos = level_.winPos;
        //player_.OnInit(randomColor);
        //dict.Remove(randomColor);

        //Setup Bot
        for (int i = 0; i < 3; i++)
        {
            Bot bot_ = Instantiate(bot);
            bot_.CurrentLevel = level_;
            characterList.Add(bot_);
            //bot_.winPos = level_.winPos;
            //bot_.OnInit(randomColor);
            //dict.Remove(randomColor);
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
        if (currentLevel == Levels.Count - 1)
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

        foreach (var level in levelList)
        {
            Destroy(level.gameObject);
        }

        foreach (var character in characterList)
        {
            Destroy(character.gameObject);
        }

        levelList.Clear();
        characterList.Clear();
    }


}
