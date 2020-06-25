using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class Audiomanager : MonoBehaviour
{
    [Header("Music")]
    [EventRef]
    [SerializeField] string Music;

    [Header("SFX")]
    [EventRef]
    [SerializeField] string powerUpWine;
    [EventRef]
    [SerializeField] string powerUpBook;
    [EventRef]
    [SerializeField] string powerUpSlam;
    [EventRef]
    [SerializeField] string powerUpSmirnoff;
    [EventRef]
    [SerializeField] string powerUpBong;
    [EventRef]
    [SerializeField] string powerUpBlood;
    [EventRef]
    [SerializeField] string powerUpBurger;
    [EventRef]
    [SerializeField] string powerUpShades;
    [EventRef]
    [SerializeField] string powerUpCat;

    [EventRef]
    [SerializeField] string footSteps;
    [EventRef]
    [SerializeField] string fall;

    [EventRef]
    [SerializeField] string jump;
    EventInstance jumpEvent;

    #region PowerUps
    public void PowerUpWine(Vector3 position)
    {
        RuntimeManager.PlayOneShot(powerUpWine, position);
    }

    public void PowerUpBook(Vector3 position)
    {
        RuntimeManager.PlayOneShot(powerUpBook, position);
    }

    public void PowerUpSlam(Vector3 position)
    {
        RuntimeManager.PlayOneShot(powerUpSlam, position);
    }

    public void PowerUpSmirnoff(Vector3 position)
    {
        RuntimeManager.PlayOneShot(powerUpSmirnoff, position);
    }

    public void PowerUpBong(Vector3 position)
    {
        RuntimeManager.PlayOneShot(powerUpBong, position);
    }

    public void PowerUpBlood(Vector3 position)
    {
        RuntimeManager.PlayOneShot(powerUpBlood, position);
    }
    public void PowerUpBurger(Vector3 position)
    {
        RuntimeManager.PlayOneShot(powerUpBurger, position);
    }
    public void PowerUpShades(Vector3 position)
    {
        RuntimeManager.PlayOneShot(powerUpShades, position);
    }
    public void PowerUpCat(Vector3 position)
    {
        RuntimeManager.PlayOneShot(powerUpCat, position);
    }
    #endregion

    public void Footsteps(Vector3 position)
    {
        RuntimeManager.PlayOneShot(footSteps, position);
    }
    public void Jump(Vector3 position)
    {
        jumpEvent = RuntimeManager.CreateInstance(jump);
        jumpEvent.start();

    }
    public void JumpCancel(Vector3 position)
    {
        jumpEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    public void Fall(Vector3 position)
    {
        RuntimeManager.PlayOneShot(fall, position);
    }
}
