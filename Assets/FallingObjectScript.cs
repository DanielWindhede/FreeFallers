using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectScript : MonoBehaviour
{
    private float _startPos;
    private GameObject _camera;
    private Vector3 _newPos;

    [SerializeField] private float _paralaxAmount;
    [SerializeField] private float _fallSpeed;
    private float fallDist;


    private void Awake()
    {
        _startPos = transform.position.y;
        _camera = GlobalState.state.Camera.gameObject;
    }

    void Update()
    {
        float paralaxDist = _camera.transform.position.y * _paralaxAmount - _fallSpeed * Time.deltaTime;
        fallDist -= _fallSpeed * Time.deltaTime;

        transform.position = new Vector3(transform.position.x, _startPos + paralaxDist + fallDist, transform.position.z);
        
        //_newPos = new Vector3(transform.position.x, _startPos + dist, transform.position.z);

        //_newPos = new Vector3(_newPos.x, _newPos.y - _fallSpeed * Time.deltaTime, _newPos.z);

        //transform.position = _newPos;
    }
}
