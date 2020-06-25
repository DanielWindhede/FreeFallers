using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{
    //flytta till global state sen

    public GlobalState.PowerupType _powerupType;

    LayerMask _playerLayers;

    private void Awake()
    {
        _playerLayers |= 1 << GlobalState.state.Players[0].layer;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_playerLayers == (_playerLayers | 1 << other.gameObject.layer))
        {
            other.GetComponent<PlayerPowerupScript>().currentPowerup = _powerupType;
            Destroy(this.gameObject);
        }
    }
}
