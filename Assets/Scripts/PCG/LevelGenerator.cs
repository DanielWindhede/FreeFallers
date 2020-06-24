using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private const int _gridWidth = 20;
    //[SerializeField] private int gridHeight;
    [SerializeField] private int _visibleHeight;
    [SerializeField] [Range(0f,1f)] private float _spawnRate = 0.2f;
    [SerializeField] private List<Obstacle> _obstacles;

    private int[,] _level;

    private int _bottomRow = 0;
    private int _reloadRow = 0;

    private int _loadingRow = 0;
    private int _loadingColumn = 0;


    // Start is called before the first frame update
    void Start()
    {
        _level = new int[_visibleHeight * 3, _gridWidth]; //[rows, columns]
        GenerateNextScreen(10);
    }

    private void GenerateNextScreen(int startRow)
    {
        for(int row = 0; row < _visibleHeight; row++)
        {
            for (int column = 0; column < _level.GetLength(1); column++)
            {
                if (_level[(startRow + row) % (_visibleHeight * 3), column] != 1 && PlaceObjectNow()) //if empty and should spawn
                {
                    _loadingRow = (startRow + row) % (_visibleHeight * 3);
                    _loadingColumn = column;

                    PlaceObstacle();
                }
            }
        }
    }
    private void PlaceObstacle()
    {
        Obstacle obstacle = _obstacles[Random.Range(0, _obstacles.Count)];

        if (CanBePlaced(obstacle))
        {

            //place obstacle
            Instantiate(obstacle, new Vector3(_loadingColumn + ((float)obstacle.size.x / 2), _bottomRow - (_loadingRow + (float)obstacle.size.y / 2), 0), new Quaternion());

            //mark occupide space
            for (int i = 0; i < obstacle.size.y; i++)
            {
                for (int j = 0; j < obstacle.size.x; j++)
                {
                    _level[_loadingRow + i, _loadingColumn + j] = 1;
                }
            }
        }
        else
        {
            PlaceObstacle();
        }
    }

    private bool PlaceObjectNow()
    {
        return Random.value < _spawnRate;
    }

    private bool CanBePlaced(Obstacle obstacle)
    {
        if (_gridWidth - _loadingColumn < obstacle.size.x)
        {
            return false;
        }
        for (int i = 0; i < obstacle.size.y; i++)
        {
            for (int j = 0; j < obstacle.size.x; j++)
            {
                if (_level[_loadingRow + i % (_visibleHeight * 3), _loadingColumn + j] == 1)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
