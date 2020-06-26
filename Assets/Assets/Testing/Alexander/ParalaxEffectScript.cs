using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxEffectScript : MonoBehaviour
{
    private float _startPos;
    [SerializeField] private float _paralaxAmount;
    private GameObject _camera;

    private void Awake()
    {
        _startPos = transform.position.y;
        _camera = GlobalState.state.Camera.gameObject;
    }

    void Update()
    {
        float dist = (_camera.transform.position.y * _paralaxAmount);
        transform.position = new Vector3(transform.position.x, _startPos + dist, transform.position.z);
    }
}
