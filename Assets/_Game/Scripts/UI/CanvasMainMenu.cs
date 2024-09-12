using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMainMenu : UICanvas
{
    [SerializeField] private Button settingButton;

    public void Play()
    {
        Close(0);
        Time.timeScale = 1;
        UIManager.Ins.OpenUI<CanvasGamePlay>().AttachJoyStick().OnInit();
        LevelManager.Ins.OnInit();
        GameManager.ChangeState(GameState.GamePlay);
    }

    public void Setting()
    {
        settingButton.gameObject.SetActive(false);
        UIManager.Ins.OpenUI<CanvasSetting>().SetButton(settingButton).OnInit(this);
    }
}
