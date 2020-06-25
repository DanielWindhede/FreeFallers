using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerPowerupScript : MonoBehaviour
{
    public GlobalState.PowerupType currentPowerup;
    
    public PlayerInput playerInput;

    [SerializeField]float _dashSpeed;
    [SerializeField] float _dashTime;

    public StateMachine<PlayerPowerupScript> powerupStateMachine;
    
    public DefaultState defaultState = new DefaultState();

    private Player2D _player2D;
    private Controller2D _controller2D;

    private GameHandler _gameHandler;

    private void OnEnable()
    {
        if (playerInput == null)
            playerInput = new PlayerInput();
        playerInput.PlayerControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.PlayerControls.Disable();
    }

    private void Awake()
    {
        currentPowerup = GlobalState.PowerupType.None;
        _gameHandler = GlobalState.state.GameHandler;


        if (playerInput == null)
            playerInput = new PlayerInput();

        playerInput.PlayerControls.UsePowerup.performed += ctx => UsePowerup();

        //_playerRB = GetComponent<Rigidbody2D>();
        _player2D = GetComponent<Player2D>();
        _controller2D = GetComponent<Controller2D>();

        powerupStateMachine = new StateMachine<PlayerPowerupScript>(this);
        defaultState = new DefaultState();

        powerupStateMachine.ChangeState(defaultState);
    }

    void Update()
    {
        powerupStateMachine.Update();
    }

    private void UsePowerup()
    {
        switch ((int)currentPowerup)
        {
            case 0:
                print("No power");
                //Destroy(this.gameObject);
                break;

            case 1:
                if (!_controller2D.collisions.below)
                {
                    print("Used nr 1");
                    currentPowerup = GlobalState.PowerupType.None;
                    powerupStateMachine.ChangeState(new DownDashState(_player2D, _dashSpeed, _dashTime));
                }
                break;

            case 2:
                print("Used nr 2");
                currentPowerup = GlobalState.PowerupType.None;
                Time.timeScale = 0;

                int teleportAmount = (int)Random.Range(4f, 8f);

                print("time to swap " + teleportAmount + " times");

                for (int i = 0; i < _gameHandler.playerList.Count; i++)
                {
                    _gameHandler.playerList[i].GetComponent<PlayerPowerupScript>().playerInput.PlayerControls.Disable();

                    StartCoroutine(_gameHandler.playerList[i].GetComponent<PlayerPowerupScript>().ZumBookTeleportShit(_gameHandler.playerList[(i + 1) % _gameHandler.playerList.Count].transform.position, 0.5f, teleportAmount));
                }
                break;
        }
    }

    public IEnumerator ZumBookTeleportShit(Vector3 newPos, float timeToWait, int teleportAmount)
    {
        if (teleportAmount > 0)
        {
            yield return new WaitForSecondsRealtime(timeToWait);
            print("swaped " + teleportAmount);

            transform.position = newPos;
            
            for (int i = 0; i < _gameHandler.playerList.Count; i++)
            {
                StartCoroutine(_gameHandler.playerList[i].GetComponent<PlayerPowerupScript>().ZumBookTeleportShit(_gameHandler.playerList[(i + 1) % _gameHandler.playerList.Count].transform.position, timeToWait * 0.8f, teleportAmount - 1));
            }
        }
        else
        {
            for (int i = 0; i < _gameHandler.playerList.Count; i++)
            {
                _gameHandler.playerList[i].GetComponent<PlayerPowerupScript>().playerInput.PlayerControls.Enable();
            }

            Time.timeScale = 1;
        }
    }
}

#region States
public class DefaultState : State<PlayerPowerupScript>
{
    public override void EnterState(PlayerPowerupScript owner)
    { }

    public override void ExitState(PlayerPowerupScript owner)
    { }

    public override void UpdateState(PlayerPowerupScript owner)
    { }
}

public class DownDashState : State<PlayerPowerupScript>
{
    //Rigidbody2D _playerRB;
    Player2D _player2D;
    float _dashSpeed;
    float _dashTime;
    Timer _timer;

    public DownDashState(/*Rigidbody2D playerRB,*/Player2D player2D, float dashSpeed, float dashTime)
    {
        _player2D = player2D;
        //_playerRB = playerRB;
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
        _timer += Time.deltaTime;

        //_playerRB.velocity = new Vector2(_playerRB.velocity.x, -_dashSpeed);
        
        _player2D.OverrideVelocity(new Vector3(0f, -_dashSpeed, 0f));

        //player2D.OverrideVelocity(new Vector3(player2D.previousVelocity.x, player2D.previousVelocity.y * _slowDownAmount, 0f));

        Debug.Log("GOOOO");
        if (_timer.Expired)
        {
            owner.powerupStateMachine.ChangeState(owner.defaultState);
        }
    }
}

public class ZumBokState : State<PlayerPowerupScript>
{
    public override void EnterState(PlayerPowerupScript owner)
    { 
        //ta alla spelarna

        //hitta en annan pos

        //swap

        //gå ut
    }

    public override void ExitState(PlayerPowerupScript owner)
    { }

    public override void UpdateState(PlayerPowerupScript owner)
    { }
}

#endregion

