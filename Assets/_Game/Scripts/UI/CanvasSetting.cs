using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSetting : UICanvas
{
    private Button settingButton;

    [SerializeField] Button closeButton;
    [SerializeField] Button closeButtonInGamePlay;
    [SerializeField] Button mainMenuButton;

    public void Close()
    {
        Time.timeScale = 1;
        settingButton.gameObject.SetActive(true);
        Close(0);
    }

    public CanvasSetting SetButton(Button settingButton)
    {
        this.settingButton = settingButton;
        return this;
    }

    public void MainMenu()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<CanvasMainMenu>();
        LevelManager.Ins.ClearLevel();
        GameManager.ChangeState(GameState.MainMenu);
    }

    public CanvasSetting OnInit(UICanvas ui)
    {
        Pause();
        if (ui is CanvasGamePlay)
        {
            mainMenuButton.gameObject.SetActive(true);
            closeButtonInGamePlay.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(false);
        }
        else
        {
            mainMenuButton.gameObject.SetActive(false);
            closeButtonInGamePlay.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);

        }
        return this;
    }
    public void Pause()
    {
        Time.timeScale = 0;

    }
}
