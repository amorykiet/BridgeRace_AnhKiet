using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : GameUnit
{
    [SerializeField] private ColorData colorData;
    [SerializeField] private MeshRenderer meshRenderer;

    private ColorType brickColor;
    public ColorType BrickColor => brickColor;

    private void Start()
    {
        OnInit(ColorType.Red);
    }

    public void OnInit(ColorType colorType)
    {
        brickColor = colorType;
        meshRenderer.material = colorData.GetMat(brickColor);
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }
}
