using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerupScript : MonoBehaviour
{
    public PowerupScript.PowerupType currentPowerup;

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

    private void Awake()
    {
        currentPowerup = PowerupScript.PowerupType.None;

        if (_playerInput == null)
            _playerInput = new PlayerInput();

        _playerInput.PlayerControls.UsePowerup.performed += ctx => UsePowerup();
    }
    void Start()
    {
        

    }

    void Update()
    {



    }

    private void UsePowerup()
    {
        print(currentPowerup);

        switch ((int)currentPowerup)
        {
            case 0:
                print("No power");
                break;
            case 1:
                print("Used nr 1");
                break;
            case 2:
                print("Used nr 2");
                break;
        }

        currentPowerup = PowerupScript.PowerupType.None;
    }
}
