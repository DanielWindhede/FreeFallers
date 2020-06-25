using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    List<GameObject> _players = new List<GameObject>();
    public void InstansiatePlayers(List<GameObject> players)
    {
        _players = players;
        for (int i = 0; i < _players.Count; i++)
        {
            Instantiate(_players[i]);
        }
        //?
    }

    public void KillPlayer(GameObject player)
    {
        //save stats?

        //destroy dead player
        _players.Remove(player);
        Destroy(player);

        //only one player left
        if (_players.Count == 1)
        {
            EndGame(_players[0]);
        }
    }

    private void EndGame(GameObject winner)
    {
        //some cool camera zoom-in

        //save stats?

        //endscreen?

        //restart / reset 2 menu?
    }
}
