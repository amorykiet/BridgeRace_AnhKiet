using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickOnStair : MonoBehaviour
{
    [SerializeField] private ColorData colorData;
    [SerializeField] private MeshRenderer meshRenderer;

    private ColorType colorType = ColorType.None;

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        meshRenderer.enabled = false;
        colorType = ColorType.None;
    }

    public bool ChangeColor(ColorType color)
    {
        if (IsSameColor(color))
        {
            return false;
        }

        meshRenderer.material = colorData.GetMat(color);

        if (colorType == ColorType.None)
        {
            meshRenderer.enabled = true;
        }

        colorType = color;
       
        return true;

    }

    public bool IsSameColor(ColorType color)
    {
        return colorType == color;   
    }
}
