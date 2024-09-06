using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {MainMenu, GamePlay, Finish}

public class GameManager : Singleton<GameManager>
{
    private static GameState gameState;

    public static void ChangeState(GameState state)
    {
        gameState = state;

        if (gameState == GameState.MainMenu)
        {
            UIManager.Ins.OpenUI<CanvasMainMenu>();
            LevelManager.Ins.ClearLevel();
        }
    }

    public static bool IsState(GameState state) => gameState == state;

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        ChangeState(GameState.MainMenu);
    }
}
