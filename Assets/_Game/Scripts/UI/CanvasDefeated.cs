using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDefeated : UICanvas
{
    [SerializeField] private Button settingButton;

    public void Setting()
    {
        settingButton.gameObject.SetActive(false);
        UIManager.Ins.OpenUI<CanvasSetting>().SetButton(settingButton).OnInit(this);
    }

    public void Retry()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<CanvasGamePlay>().OnInit();
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
