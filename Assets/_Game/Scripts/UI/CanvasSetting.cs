using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSetting : UICanvas
{
    private Button settingButton;

    public void Close()
    {
        settingButton.gameObject.SetActive(true);
        Close(0);
    }

    public void SetButton(Button settingButton)
    {
        this.settingButton = settingButton;
    }
}
