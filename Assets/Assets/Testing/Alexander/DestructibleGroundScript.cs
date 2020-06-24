﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleGroundScript : MonoBehaviour
{


    [SerializeField] float _velocityToBreak;

    
    [SerializeField] [Range(0, 1)] float _slowDownAmount;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (_playerLayers == (_playerLayers | 1 << collision.gameObject.layer))
        {
            //print(collision.rigidbody.velocity.y);
            print("impact velocity: " + Mathf.Abs(collision.relativeVelocity.y));
            
            if (Mathf.Abs(collision.relativeVelocity.y) > _velocityToBreak)
            {

                //starta en coroutine som deletar bitarna efter x sec   
                collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(collision.gameObject.GetComponent<Rigidbody>().velocity.x, collision.relativeVelocity.y * _slowDownAmount, 0f);
                
                Destroy(this.gameObject);
            }
        }
    }



}
