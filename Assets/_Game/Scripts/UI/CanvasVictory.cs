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
        LevelManager.Ins.LoadLevel();
        GameManager.ChangeState(GameState.GamePlay);
    }    

    public void Retry()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<CanvasGamePlay>();
        LevelManager.Ins.ReloadLevel();
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
