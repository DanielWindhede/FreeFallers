using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField] [Range(0.01f, 1)] float _folowAmount;
    [SerializeField] float _folowSpeed;

    [SerializeField] float _cloudSpawnDistanceBase;
    [SerializeField] float _cloudSpawnDistanceRandInc;
    float _nextCloudSpawnPosY;
    float _lastCloudSpawnPosY;
    
    [SerializeField] float _fallingObjectSpawnDistanceBase;
    [SerializeField] float _fallingObjecSpawnDistanceRandInc;
    float _nextFallingObjecSpawnPosY;
    float _lastFallingObjecSpawnPosY;




    Camera _camera;
    [SerializeField] BackgroundSpawnerScript _cloudSpawner;
    [SerializeField] BackgroundSpawnerScript _fallingObjectSpawner;

    List<GameObject> _players = new List<GameObject>();

    //fixa en lista av spelare som man kan följa sen när vi har fler
    [SerializeField] GameObject _playerToFolow;


    private void Awake()
    {
        _camera = GlobalState.state.Camera;
        _lastCloudSpawnPosY = transform.position.y;
        _nextCloudSpawnPosY = _lastCloudSpawnPosY - _cloudSpawnDistanceBase - Random.Range(0f, _cloudSpawnDistanceRandInc);
        print(_lastCloudSpawnPosY);
        print(_nextCloudSpawnPosY);

    }

    private void Start()
    {
        _players = GlobalState.state.Players;
    }

    private void FixedUpdate()
    {
        _playerToFolow = _players[0];
        for (int i = 0; i < _players.Count - 1; i++)
        {
            if (_playerToFolow.transform.position.y > _players[i + 1].transform.position.y)
            {
                _playerToFolow = _players[i + 1];
            }
        }

        //if (_playerToFolow.transform.position.y < transform.position.y)
        if (_playerToFolow.transform.position.y + _camera.orthographicSize * _folowAmount < transform.position.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, _playerToFolow.transform.position.y + _camera.orthographicSize * _folowAmount, -10f), _folowSpeed);
            _lastCloudSpawnPosY = transform.position.y;
            _lastFallingObjecSpawnPosY = transform.position.y;

            if (_lastCloudSpawnPosY < _nextFallingObjecSpawnPosY)
            {
                _cloudSpawner.spawnBackgroundObject();
                _nextCloudSpawnPosY = _lastCloudSpawnPosY - _cloudSpawnDistanceBase - Random.Range(0f, _cloudSpawnDistanceRandInc);
            }
            
            if (_lastFallingObjecSpawnPosY < _nextFallingObjecSpawnPosY)
            {
                _fallingObjectSpawner.spawnBackgroundObject();

                _nextFallingObjecSpawnPosY = _lastFallingObjecSpawnPosY - _fallingObjectSpawnDistanceBase - Random.Range(0f, _fallingObjecSpawnDistanceRandInc);
            }
        }
    }
}
