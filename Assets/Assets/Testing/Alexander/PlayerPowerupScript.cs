using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerPowerupScript : MonoBehaviour
{
    public PowerupScript.PowerupType currentPowerup;
    
    private PlayerInput _playerInput;

    private Rigidbody2D _playerRB;

    [SerializeField]float _dashSpeed;
    [SerializeField] float _dashTime;

    public StateMachine<PlayerPowerupScript> powerupStateMachine;
    
    public DefaultState defaultState = new DefaultState();
    //public DownDashState downDashState = new DownDashState();

    
    

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

        _playerRB = GetComponent<Rigidbody2D>();

        powerupStateMachine = new StateMachine<PlayerPowerupScript>(this);
        defaultState = new DefaultState();

        powerupStateMachine.ChangeState(defaultState);
    }
    void Start()
    {
        
    }

    void Update()
    {
        //print(GetComponent<Rigidbody>().velocity.y);

        powerupStateMachine.Update();
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
                //kolla att man är i luften 
                print("Used nr 1");
                powerupStateMachine.ChangeState(new DownDashState(_playerRB, _dashSpeed, _dashTime));
                break;
            case 2:
                print("Used nr 2");
                break;
        }

        //flytta sen
        currentPowerup = PowerupScript.PowerupType.None;
    }
}

public class DefaultState : State<PlayerPowerupScript>
{
    public DefaultState()
    {

    }

    public override void EnterState(PlayerPowerupScript owner)
    {

    }

    public override void ExitState(PlayerPowerupScript owner)
    {

    }

    public override void UpdateState(PlayerPowerupScript owner)
    {

    }
}

public class DownDashState : State<PlayerPowerupScript>
{
    Rigidbody2D _playerRB;
    float _dashSpeed;
    float _dashTime;
    Timer _timer;

    public DownDashState(Rigidbody2D playerRB, float dashSpeed, float dashTime)
    {
        _playerRB = playerRB;
        _dashSpeed = dashSpeed;
        _dashTime = dashTime;
    }

    public override void EnterState(PlayerPowerupScript owner)
    {
        _timer = new Timer(_dashTime);
    }

    public override void ExitState(PlayerPowerupScript owner)
    {
        //owner.currentPowerup = PowerupScript.PowerupType.None;
    }

    public override void UpdateState(PlayerPowerupScript owner)
    {
        _playerRB.velocity = new Vector2(_playerRB.velocity.x, -_dashSpeed);
        Debug.Log("GOOOO");
        if (_timer.Expired)
        {
            owner.powerupStateMachine.ChangeState(owner.defaultState);
        }
    }
}

