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
        //_player = GlobalState.state.Player.gameObject;
        _playerLayers |= 1 << _player.layer;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("meme");

        if (_playerLayers == (_playerLayers | 1 << collision.gameObject.layer))
        {
            //print(collision.rigidbody.velocity.y);
            //print("impact velocity: " + Mathf.Abs(this.GetComponent<Rigidbody2D>().velocity.y));
            Player2D player2D = collision.gameObject.GetComponent<Player2D>();

            print("impact velocity: " + player2D.previousVelocity.y);

            if (Mathf.Abs(player2D.previousVelocity.y) > _velocityToBreak)
            {
                //starta en coroutine som deletar bitarna efter x sec
                //collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x, collision.relativeVelocity.y * _slowDownAmount);

                player2D.OverrideVelocity(new Vector3(player2D.previousVelocity.x, player2D.previousVelocity.y * _slowDownAmount, 0f));

                Instantiate(_brokenBoardPrefab, transform.position, this.transform.rotation);


                Destroy(this.gameObject);
            }
        }
    }
    
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    print("meme");

    //    if (_playerLayers == (_playerLayers | 1 << collision.gameObject.layer))
    //    {
    //        //print(collision.rigidbody.velocity.y);
    //        print("impact velocity: " + Mathf.Abs(collision.relativeVelocity.y));
            
    //        if (Mathf.Abs(collision.relativeVelocity.y) > _velocityToBreak)
    //        {
    //            //starta en coroutine som deletar bitarna efter x sec
    //            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x, collision.relativeVelocity.y * _slowDownAmount);

    //            Instantiate(_brokenBoardPrefab, transform.position, Quaternion.identity);


    //            Destroy(this.gameObject);
    //        }
    //    }
    //}
}
