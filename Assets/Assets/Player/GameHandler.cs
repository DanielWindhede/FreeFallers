using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameHandler : MonoBehaviour
{

    [NonSerialized] public List<GameObject> playerList = new List<GameObject>();

    private void Awake()
    {
        playerList = GlobalState.state.Players;

        //InstansiatePlayers();

    }
    private void Start()
    {
        //KillPlayer(_players[1]);
    }
    public void InstansiatePlayers()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Instantiate(playerList[i]);
        }
        //?
    }

    public void RemovePlayer(GameObject player)
    {
        //cool esplosion ting

        //save stats?

        //destroy dead player
        playerList.Remove(player);
        //Destroy(player);

        //only one player left
        if (playerList.Count <= 1)
        {
            if (playerList.Count == 1)
            {
                EndGame(playerList[0]);
            }
            else
            {
                EndGame();
            }
        }
    }

    private void EndGame(GameObject winner)
    {

        print( winner.gameObject.name + "amma weener ");

        SceneManager.LoadScene(1);
        
        //SOME1 WIIBNER

        //some cool camera zoom-in

        //save stats?

        //endscreen? (med winner)

        //restart / reset 2 menu?
    }
    
    private void EndGame()
    {
        print("Issa draw ");
        SceneManager.LoadScene(2);

        //ITS A DRAW (NO CONTEST (AWWWWWW))

        //some cool camera zoom-in

        //save stats?

        //endscreen? (is draw)

        //restart / reset 2 menu?
    }
}
