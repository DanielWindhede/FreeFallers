using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleGroundScript : MonoBehaviour
{


    [SerializeField] float _velocityToBreak;

    [SerializeField] GameObject _brokenBoardPrefab;

    
    [SerializeField] [Range(0, 1)] float _slowDownAmount;

    //använd global state sen
    [SerializeField] GameObject _player;
    LayerMask _playerLayers;

    private void Awake()
    {
        _playerLayers |= 1 << _player.layer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_playerLayers == (_playerLayers | 1 << collision.gameObject.layer))
        {
            Player2D player2D = collision.gameObject.GetComponent<Player2D>();

            if (Mathf.Abs(player2D.previousVelocity.y) > _velocityToBreak)
            {
                player2D.OverrideVelocity(new Vector3(player2D.previousVelocity.x, player2D.previousVelocity.y * _slowDownAmount, 0f));

                Instantiate(_brokenBoardPrefab, transform.position, this.transform.rotation);

                GlobalState.state.AudioManager.FloorBreak(transform.position);
                Destroy(this.gameObject);
            }

        }
    }
}
