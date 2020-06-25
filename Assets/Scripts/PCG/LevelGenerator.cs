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

    
    private int _topPosition = 0; //y-coordinate at the top of the screen to be loaded


    private int _bottomPosition = 9; //World Space y-coordinate

    private int _loadingRow = 0; //matrix row
    private int _screenRow = 0;
    private int _loadingColumn = 0; //matrix column



    // Start is called before the first frame update
    void Start()
    {
        _level = new int[_visibleHeight * 3, _gridWidth]; //[rows, columns]

        

        GenerateNextScreen();
    }

    private void GenerateNextScreen()
    {
        for(int row = 0; row < _visibleHeight; row++)
        {
            for (int column = 0; column < _level.GetLength(1); column++)
            {
                if (_level[(-_topPosition + row) % (_visibleHeight * 3), column] != 1 && PlaceObjectNow()) //if empty and should spawn
                {
                    _loadingRow = (-_topPosition + row) % (_visibleHeight * 3);
                    _screenRow = row;
                    _loadingColumn = column;

                    PlaceObstacle();
                }
            }
        }
        _topPosition -= _visibleHeight;
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
                if (_level[(_loadingRow + i) % (_visibleHeight * 3), _loadingColumn + j] == 1)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (/*collision.gameObject.GetComponent<Player2D>()*/ collision.gameObject.tag == "Player")
        {
            GenerateNextScreen();
            GetComponent<BoxCollider2D>().transform.Translate(Vector3.down * _visibleHeight);

            RaycastHit2D[] hits = Physics2D.BoxCastAll((Vector2)transform.position + new Vector2(0, _visibleHeight * 2), new Vector2(_gridWidth, _visibleHeight), 0, Vector2.up, 0.5f);

            foreach (RaycastHit2D hit in hits)
            {
                Destroy(hit.collider.gameObject);
            }

            for (int i = 0; i < _visibleHeight; i++)
            {
                int row = (_loadingRow + _visibleHeight / 2 + i) % (_visibleHeight * 3);
                for (int j = 0; j < _gridWidth; j++)
                {
                    _level[row , j] = 0;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0, 0.2f);

        Gizmos.DrawCube(transform.position, new Vector3(_gridWidth, _visibleHeight, 1));
    }
}
