using Scriptable;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public FixedJoystick joystick;

    [SerializeField] private List<Level> Levels = new();
    [SerializeField] private Player player;
    [SerializeField] private Bot bot;
    [SerializeField] private CameraFollow cam;

    private List<Level> currentLevelList = new();
    private List<Player> currentPlayerList = new();
    private List<Bot> currentBotList = new();

    private int currentLevel;

    
    private void OnEnable()
    {
        Player.onPlayerWin += CompleteLevel;
        Bot.onBotWin += FailLevel;
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
        currentLevelList.Add(level_);
        Dictionary<ColorType, Vector3> dict = level_.characterPosDictionary;

        //Setup Player
        Player player_ = Instantiate(player);
        player_.winPos = level_.winPos;
        player_.joyStick = joystick;
        ColorType randomColor = dict.ElementAt(Random.Range(0, dict.Count)).Key;
        player_.OnInit(randomColor);
        player_.transform.position = dict[randomColor];
        dict.Remove(randomColor);
        currentPlayerList.Add(player_);
        cam.FollowToTarget(player_.transform);

        //Setup Bot
        for (int i = 0; i < 3; i++)
        {
            randomColor = dict.ElementAt(Random.Range(0, dict.Count)).Key;
            Bot bot_ = Instantiate(bot, dict[randomColor], Quaternion.identity);
            bot_.winPos = level_.winPos;
            bot_.CurrentLevel = level_;
            bot_.OnInit(randomColor);
            dict.Remove(randomColor);
            currentBotList.Add(bot_);
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
        foreach (var level in currentLevelList)
        {
            Destroy(level.gameObject);
        }

        foreach (var player in currentPlayerList)
        {
            Destroy(player.gameObject);
        }

        foreach (var bot in currentBotList)
        {
            Destroy(bot.gameObject);
        }
        currentLevelList.Clear();
        currentPlayerList.Clear();
        currentBotList.Clear();
    }

    private void StopCharacterMove()
    {
        foreach (Player player in currentPlayerList)
        {
            player.Stop();
        }

        foreach (Bot bot in currentBotList)
        {
            bot.Stop();
        }

    }

    private void CompleteLevel()
    {
        PlayerPrefs.SetInt("currentLevel", currentLevel);
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
}
