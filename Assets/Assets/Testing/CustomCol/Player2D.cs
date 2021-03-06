﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player2D : MonoBehaviour
{
    // Stats
    [SerializeField] private float _maxJumpHeight = 4;
    [SerializeField] private float _minJumpHeight = 2;
    [SerializeField] private float _timeToJumpApex = 0.4f;
    private float _accelerationTimeAirborne = 0.2f;
    private float _accelerationTimeGrounded = 0.1f;
    private float _moveSpeed = 6;

    // Info
    private float _gravity;
    [NonSerialized] public float _gravityMultiplier;
    private float _maxJumpVelocity;
    private float _minJumpVelocity;
    private Vector3 _velocity;
    [NonSerialized] public Vector3 previousVelocity;
    private float _velocityXSmoothing;

    // Jump
    private int _jumpType;
    private bool _jumped;

    // Other
    private bool _overrideVelocity;
    private Vector2 _tempVelocity;
    private Controller2D _controller;
    private Vector2 _inputDirection;
    private PlayerInput _playerInput;
    [SerializeField] private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void OnEnable() { _playerInput.PlayerControls.Enable(); }
    private void OnDisable() { _playerInput.PlayerControls.Disable(); }

    void Awake()
    {
        // Initialization
        _controller = GetComponent<Controller2D>();
        _playerInput = new PlayerInput();
        _spriteRenderer = _animator.GetComponent<SpriteRenderer>();

        _gravityMultiplier = 1f;
        _gravity = -(2 * _maxJumpHeight) / Mathf.Pow(_timeToJumpApex, 2);
        _maxJumpVelocity = Mathf.Abs(_gravity) * _timeToJumpApex;
        _minJumpVelocity = Mathf.Sqrt(Mathf.Abs(_gravity) * _minJumpHeight);

        // Input
        _playerInput.PlayerControls.Move.performed += ctx => _inputDirection = ctx.ReadValue<Vector2>();
        _playerInput.PlayerControls.Jump.started += ctx => _jumpType = 1;
        _playerInput.PlayerControls.Jump.canceled += ctx => _jumpType = 2;
        _playerInput.PlayerControls.Pause.performed += ctx => MouseExplosion();


        DontDestroyOnLoad(this.gameObject);
    }



    void MouseExplosion()
    {
        Vector2 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 finalDir = ((Vector2)transform.position - camPos).normalized;
        OverrideVelocity(finalDir * 1000 * Time.deltaTime);
        //OverrideVelocity(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * 1000 * Time.deltaTime);
    }

    private void Update()
    {
        if (_animator)
        {
            _animator.SetBool("Grounded", _controller.collisions.below);
            if (Mathf.Abs(_velocity.x) < 0.1f)
                _animator.SetBool("Running", false);
            else
            {
                _animator.SetBool("Running", true);
                _spriteRenderer.flipX = ((Mathf.Sign(_velocity.x) >= 0) ? true : false);
            }
        }
    }

    void FixedUpdate()
    {
        previousVelocity = _velocity;

        if (_overrideVelocity)
        {
            _velocity = _tempVelocity;
            _overrideVelocity = false;
        }

        if (_controller.collisions.below)
        {
            _jumped = false;
        }

        if (_jumpType > 0)
        {
            if (_jumpType == 1)
            {
                if (_controller.collisions.below && !_jumped)
                {
                    if (_controller.collisions.slidingDownMaxSlope)
                    {
                        if (_inputDirection.x != -Mathf.Sign(_controller.collisions.slopeNormal.x))
                        { // not jumping against max slope
                            _velocity.y = /* _maxJumpVelocity * */ _controller.collisions.slopeNormal.y * _velocity.magnitude * 2;
                            _velocity.x =  /* _maxJumpVelocity * */  _controller.collisions.slopeNormal.x * _velocity.magnitude * 2;
                        }
                    }
                    else
                    {
                        _velocity.y = _maxJumpVelocity;
                    }
                    _jumped = true;
                    _animator.SetTrigger("Jump");
                }
            }
            else
            {
                if (_velocity.y > _minJumpVelocity && _jumped)
                {
                    _velocity.y = _minJumpVelocity;
                }
            }
            _jumpType = 0;
        }

        float targetVelocityX = _inputDirection.x * _moveSpeed;
        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing, (_controller.collisions.below) ? _accelerationTimeGrounded : _accelerationTimeAirborne);
        
        if (_velocity.y < 0)
        {
            _velocity.y += _gravity * Time.fixedDeltaTime * _gravityMultiplier;
        }
        else
        {
            _velocity.y += _gravity * Time.fixedDeltaTime;
        }
        
        _controller.Move(_velocity * Time.fixedDeltaTime);

        if (_controller.collisions.above || _controller.collisions.below)
        {
            if (_controller.collisions.slidingDownMaxSlope)
            {
                _velocity.y += _controller.collisions.slopeNormal.y * -_gravity * Time.fixedDeltaTime;
            }
            else
            {
                _velocity.y = 0;
            }
        }
    }

    private void OnDestroy()
    {
        print(GlobalState.state);
        print(GlobalState.state.GameHandler);
        GlobalState.state.GameHandler.RemovePlayer(this.gameObject);
    }

    public void OverrideVelocity(Vector3 amount)
    {
        _tempVelocity = amount;
        _overrideVelocity = true;
    }
}