using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasVictory : UICanvas
{
    [SerializeField] private Button settingButton;

    public void Setting()
    {
        settingButton.gameObject.SetActive(false);
        UIManager.Ins.OpenUI<CanvasSetting>().SetButton(settingButton);
    }

    public void NextLevel()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<CanvasGamePlay>();
        LevelManager.Ins.ClearLevel();
        LevelManager.Ins.LoadNextLevel();
        GameManager.ChangeState(GameState.GamePlay);
    }    

    public void Retry()
    {
        LevelManager.Ins.ReloadLevel();
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<CanvasGamePlay>();
        GameManager.ChangeState(GameState.GamePlay);
    }

    public void MainMenu()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<CanvasMainMenu>();
        LevelManager.Ins.ClearLevel();
        GameManager.ChangeState(GameState.MainMenu);
    }
}
