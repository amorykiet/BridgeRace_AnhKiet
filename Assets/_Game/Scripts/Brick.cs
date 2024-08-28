using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : GameUnit
{
    [SerializeField] private ColorData colorData;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private ColorType brickColor;
    public ColorType BrickColor => brickColor;

    //Test
    private void Start()
    {
        OnInit(brickColor);
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
