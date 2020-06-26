using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject[] _backgroundObjectsToSpawn;

    Camera _camera;
    float _cameraWidth;

    private void Awake()
    {
        _camera = GlobalState.state.Camera;

        _cameraWidth = _camera.orthographicSize * Screen.width / Screen.height;
        
    }

    void Start()
    {
        
    }


    void Update()
    {
        //spawnRandomBackgroundObjectAtRandomPosition();
    }

    public void spawnRandomBackgroundObjectAtRandomPosition()
    {
        int index = Random.Range(0, _backgroundObjectsToSpawn.Length);

        float spawnPosX = Random.Range(-_cameraWidth, _cameraWidth);

        Instantiate(_backgroundObjectsToSpawn[index], new Vector3(gameObject.transform.position.x + spawnPosX, gameObject.transform.position.y, 0f), Quaternion.identity);
    }
}
