using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] private FixedJoystick joystick;


    public void AttachJoyStick()
    {
        LevelManager.Ins.joystick = joystick;
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

}
