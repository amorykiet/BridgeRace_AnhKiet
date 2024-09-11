using Scriptable;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector2 spawnByAxis;
    [SerializeField] private int colorNumber = 1;
    [SerializeField] private float horizontalDistance;
    [SerializeField] private float verticalDistance;

    public int MaxBricksPerColor => (int) spawnByAxis.x * (int) spawnByAxis.y / colorNumber;

    private Dictionary<ColorType, int> colorAvailableDict = new();

    private List<Brick> brickList = new();

    public void SpawnBrick()
    {
        for (int i = 0; i < colorNumber; i++)
        {
            // Carefull when ColorType enum is changed
            colorAvailableDict.Add((ColorType) (i + 1), 0);
        }

        for (int i = 0; i < spawnByAxis.x; i++)
        {
            for(int j = 0; j < spawnByAxis.y; j++)
            {
                ColorType randomColor = colorAvailableDict.ElementAt(Random.Range(0, colorAvailableDict.Count)).Key;
                colorAvailableDict[randomColor]++;
                if(colorAvailableDict[randomColor] >= MaxBricksPerColor)
                {
                    colorAvailableDict.Remove(randomColor);
                }
                Brick temp = SimplePool.Spawn<Brick>(PoolType.Brick, spawnPoint.position + new Vector3(i*horizontalDistance, 0, j*verticalDistance), spawnPoint.rotation);
                temp.OnInit(randomColor);
                brickList.Add(temp);
            }
        }
    }

    public void SpawnDeactiveBrick()
    {
        if (brickList.Count <= 0)
        {
            SpawnBrick();
        }

        foreach (Brick brick in brickList)
        {
            brick.DeActivate();
        }

    }
    
    public void SpawnBrickByColor(ColorType color)
    {
        if (brickList.Count <= 0)
        {
            SpawnDeactiveBrick();
        }

        foreach (Brick brick in brickList)
        {
            if(brick.BrickColor == color)
            {
                brick.Activate();
            }
        }
    }

    public Vector3 GetRandomBrickPosition()
    {
        int x = Random.Range(0, (int)spawnByAxis.x) * (int)horizontalDistance;
        int y = Random.Range(0, (int)spawnByAxis.y) * (int)verticalDistance;
        Vector3 randomPosition = spawnPoint.position + new Vector3(x, 0, y);
        return randomPosition;
    }

}
