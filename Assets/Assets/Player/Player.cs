using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 _inputDirection;
    private PlayerInput _playerInput;

    private void OnEnable()
    {
        if (_playerInput == null)
            _playerInput = new PlayerInput();
        _playerInput.PlayerControls.Enable();
    }

    private void OnDisable()
    {
        _playerInput.PlayerControls.Disable();
    }
    
    void Awake()
    {
        _playerInput.PlayerControls.Move.performed += ctx => _inputDirection = ctx.ReadValue<Vector2>();
        _playerInput.PlayerControls.Jump.performed += ctx => Jump();
        //_playerInput.PlayerControls.Pause.performed += ctx => ;
    }

    private void Jump()
    {

    }
}
