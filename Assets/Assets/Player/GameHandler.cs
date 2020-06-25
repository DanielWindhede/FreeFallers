﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    List<GameObject> _players = new List<GameObject>();

    private void Awake()
    {
        _players = GlobalState.state.Players;

        //InstansiatePlayers();

    }
    private void Start()
    {
        //KillPlayer(_players[1]);
    }
    public void InstansiatePlayers()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            Instantiate(_players[i]);
        }
        //?
    }

    public void RemovePlayer(GameObject player)
    {
        //cool esplosion ting

        //save stats?

        //destroy dead player
        _players.Remove(player);
        //Destroy(player);

        //only one player left
        if (_players.Count <= 1)
        {
            if (_players.Count == 1)
            {
                EndGame(_players[0]);
            }
            else
            {
                EndGame();
            }
        }
    }

    private void EndGame(GameObject winner)
    {
        print("amma weener ");
        //SOME1 WIIBNER

        //some cool camera zoom-in

        //save stats?

        //endscreen? (med winner)

        //restart / reset 2 menu?
    }
    
    private void EndGame()
    {
        print("Issa draw ");
        //ITS A DRAW (NO CONTEST (AWWWWWW))

        //some cool camera zoom-in

        //save stats?

        //endscreen? (is draw)

        //restart / reset 2 menu?
    }
}
