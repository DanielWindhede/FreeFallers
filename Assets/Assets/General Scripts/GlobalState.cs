using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalState : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private GameObject _player;
    [SerializeField] private List<GameObject> _players = new List<GameObject>();

    [SerializeField] private Camera _camera;

    [SerializeField] private GameHandler _gameHandler;

    [SerializeField] private Audiomanager _audiomanager;

    //[SerializeField] private AudioManager _audioManager;

    public enum PowerupType
    {
        None,
        GroundSlam,
        ZumBok,
        Smirnoof,
        Burger
    };

    //public PowerupType powerupType;


    public List<GameObject> Players
    {
        get { return _players; }
    }

    public Camera Camera
    {
        get { return _camera; }
    }

    public GameHandler GameHandler

    {

        get { return _gameHandler; }

    }



    public Audiomanager AudioManager
    {
        get { return _audiomanager; }
    }

    private static GlobalState _state;
    public static GlobalState state {
        get {
            if (_state == null)
            {
                _state = GameObject.FindObjectOfType<GlobalState>();
            }
            return _state;
        }
    }

    private void Awake()
    {
        if (_state != null && _state != this) {
            Destroy(this.gameObject);
        }
        else {
            // DontDestroyOnLoad(this);
            _state = this;
        }
    }
    private bool _hitstopRunning;
    public void Hitstop(float duration)
    {
        if (!_hitstopRunning && duration > 0)
            StartCoroutine(HitstopCoroutine(duration));
    }

    private IEnumerator HitstopCoroutine(float duration)
    {
        _hitstopRunning = true;
        yield return new WaitForEndOfFrame();

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        _hitstopRunning = false;

        yield return 0;
    }

    private void OnDestroy()
    {
        if (this == _state)
        {
            _state = null;
        }
    }
}