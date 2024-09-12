using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Button settingButton;

    public CanvasGamePlay AttachJoyStick()
    {
        LevelManager.Ins.joystick = joystick;
        return this;
    }

    public void PlayerWin()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<CanvasVictory>();
    }

    public void PlayerLose()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<CanvasDefeated>();
    }

    public void Setting()
    {
        settingButton.gameObject.SetActive(false);
        UIManager.Ins.OpenUI<CanvasSetting>().SetButton(settingButton).OnInit(this);
    }

    new public CanvasGamePlay OnInit()
    {
        base.OnInit();
        settingButton.gameObject.SetActive(true);
        return this;
    }

}
