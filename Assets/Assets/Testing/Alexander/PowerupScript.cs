using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{
    //flytta till global state sen
    [SerializeField]
    public enum PowerupType
    {
        None,
        PowerupType1,
        PowerupType2
    };

    public PowerupType powerupType;


    //använd global state sen
    [SerializeField] GameObject _player;
    LayerMask _playerLayers;

    private void Awake()
    {
        //_player = GlobalState.state.Player.gameObject;
        _playerLayers |= 1 << _player.layer;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (_playerLayers == (_playerLayers | 1 << other.gameObject.layer))
    //    {
    //        other.GetComponent<PlayerPowerupScript>().currentPowerup = powerupType;
    //        Destroy(this.gameObject);
    //    }
    //}
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_playerLayers == (_playerLayers | 1 << other.gameObject.layer))
        {
            other.GetComponent<PlayerPowerupScript>().currentPowerup = powerupType;
            Destroy(this.gameObject);
        }
    }
}
