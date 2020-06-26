using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkrapanScript : MonoBehaviour
{
    private float _startPos;
    private float _height;
    [SerializeField] private float _paralaxAmount;
    private GameObject _camera;

    private void Awake()
    {
        _startPos = transform.position.y;
        _height = GetComponent<SpriteRenderer>().bounds.size.y;
        _camera = GlobalState.state.Camera.gameObject;
    }

    void Update()
    {
        float e = _camera.transform.position.y * (1 - _paralaxAmount);

        float dist = _camera.transform.position.y * _paralaxAmount;

        transform.position = new Vector3(transform.position.x, _startPos + dist, transform.position.z);

        if(e < _startPos - _height)
        {
            _startPos -= _height * 2;
        }
    }
}
