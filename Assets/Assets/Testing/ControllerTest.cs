using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTest : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private PhysicsMaterial2D _noFrictionMat;
    [SerializeField] private PhysicsMaterial2D _fullFrictionMat;

    private Rigidbody2D _body;


    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float m = Input.GetAxisRaw("Horizontal");

        _body.velocity = new Vector2(m * _speed, _body.velocity.y);

        if (m > 0)
            transform.eulerAngles = Vector3.zero;
        else if (m < 0)
            transform.eulerAngles = Vector3.up * 180;

        
        if (m != 0)
            _body.sharedMaterial = _noFrictionMat;
        else
            _body.sharedMaterial = _fullFrictionMat;
            
    }
}
