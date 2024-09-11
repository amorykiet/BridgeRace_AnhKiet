using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : GameUnit
{
    public ColorType BrickColor => brickColor;

    [SerializeField] private ColorData colorData;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ColorType brickColor;
    [SerializeField] private Collider brickCollider;
    [SerializeField] private GameObject renderObject;

    private float timeToSpawn = 6;
    private float timeCount = 0;
    private bool counting = false;


    private void Update()
    {
        if (!counting) { return; }
        if (timeCount < timeToSpawn)
        {
            timeCount += Time.deltaTime;
        }
        else
        {
            OnSpawn();
        }
    }


    public void OnInit(ColorType colorType)
    {
        brickColor = colorType;
        meshRenderer.material = colorData.GetMat(brickColor);
    }

    //OnDespawn that setup to spawn again
    public void OnDespawnToSpawn()
    {
        timeCount = 0;
        counting = true;
        brickCollider.enabled = false;
        renderObject.SetActive(false);
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }

    private void OnSpawn()
    {
        counting = false;
        brickCollider.enabled = true;
        renderObject.SetActive(true);
    }

}
