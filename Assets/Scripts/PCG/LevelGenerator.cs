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
    [SerializeField] private List<GameObject> _powerUps;
    [SerializeField] [Range(0, 0.5f)] private float _powerUpSpawnRate;

    private int[,] _level;

    
    private int _topPosition = 0; //y-coordinate at the top of the screen to be loaded

    private int _nextLoadPosition = 0; //World Space y-coordinate the camera has to pass to load next screen
    private Camera _camera;

    private int _loadingRow = 0; //matrix row
    private int _screenRow = 0;
    private int _loadingColumn = 0; //matrix column



    // Start is called before the first frame update
    void Start()
    {
        _level = new int[_visibleHeight * 5, _gridWidth]; //[rows, columns]
        _nextLoadPosition = -_visibleHeight / 2;
        _camera = Camera.main;

        GenerateNextScreen();
        GenerateNextScreen();
        GenerateNextScreen();
    }

    private void GenerateNextScreen()
    {
        for(int row = 0; row < _visibleHeight; row++)
        {
            for (int column = 0; column < _level.GetLength(1); column++)
            {
                if (_level[(-_topPosition + row) % (_visibleHeight * 3), column] != 1 && SpawRateTest(_spawnRate)) //if empty and should spawn
                {
                    _loadingRow = (-_topPosition + row) % (_visibleHeight * 3);
                    _screenRow = row;
                    _loadingColumn = column;

                    if (SpawRateTest(_powerUpSpawnRate))
                    {
                        PlacePowerUp();
                    }
                    else
                    {
                        PlaceObstacle();
                    }


                }
            }
        }
        _topPosition -= _visibleHeight;
    }

    private void PlacePowerUp()
    {
        Instantiate(_powerUps[Random.Range(0, _powerUps.Count)], new Vector3(_loadingColumn + 0.5f - _gridWidth / 2, _topPosition - (_screenRow + 0.5f), 0), new Quaternion());
    }

    private void PlaceObstacle()
    {
        Obstacle obstacle = _obstacles[Random.Range(0, _obstacles.Count)];

        if (CanBePlaced(obstacle))
        {

            //place obstacle
            Instantiate(obstacle, new Vector3(_loadingColumn + ((float)obstacle.size.x / 2) - _gridWidth / 2, _topPosition -(_screenRow + (float)obstacle.size.y / 2), 0), new Quaternion());

            //mark occupide space
            for (int i = 0; i < obstacle.size.y; i++)
            {
                for (int j = 0; j < obstacle.size.x; j++)
                {
                    _level[(_loadingRow + i) % (_visibleHeight * 3), _loadingColumn + j] = 1;
                }
            }
        }
        else
        {
            PlaceObstacle();
        }
    }

    private bool SpawRateTest(float spawnRate)
    {
        return Random.value < spawnRate;
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
                if (_level[(_loadingRow + i) % (_visibleHeight * 3), _loadingColumn + j] == 1)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void FixedUpdate()
    {
        if (_camera.transform.position.y < _nextLoadPosition)
        {
            GenerateNextScreen();
            MoveDown();
        }
    }

    private void MoveDown()
    {      
        transform.Translate(Vector3.down * _visibleHeight);
        _nextLoadPosition -= _visibleHeight;

        RaycastHit2D[] hits = Physics2D.BoxCastAll((Vector2)transform.position + new Vector2(0, _visibleHeight * 2), new Vector2(_gridWidth, _visibleHeight), 0, Vector2.up, 0.5f);

        foreach (RaycastHit2D hit in hits)
        {
            Destroy(hit.collider.gameObject);
        }

        //empty out matrix under
        for (int i = 0; i < _visibleHeight; i++)
        {
            int row = (_loadingRow + _visibleHeight / 2 + i) % (_visibleHeight * 3);
            for (int j = 0; j < _gridWidth; j++)
            {
                _level[row, j] = 0;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0, 0.2f);

        Gizmos.DrawCube(transform.position, new Vector3(_gridWidth, _visibleHeight, 1));
    }
}
