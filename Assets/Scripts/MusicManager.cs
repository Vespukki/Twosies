using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    static MusicManager _instance;
    public static MusicManager Instance => _instance;

    [SerializeField] AudioSource jump;
    [SerializeField] AudioSource buttonDown;
    [SerializeField] AudioSource buttonUp;
    [SerializeField] AudioSource fall;
    [SerializeField] AudioSource restart;
    [SerializeField] AudioSource portal;

    public int deaths;
    public float time;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        PlayerMovement.OnJump += PlayJump;
        ButtonStateBehavior.OnDown += PlayButtonDown;
        ButtonStateBehavior.OnUp += PlayButtonUp;
        Block.OnFall += PlayFall;
        PlayerMovement.OnRestart += PlayRestart;
        Portal.OnPortal += PlayPortal;

        deaths = 0;
    }

    private void OnDestroy()
    {
        PlayerMovement.OnJump -= PlayJump;
        ButtonStateBehavior.OnDown -= PlayButtonDown;
        ButtonStateBehavior.OnUp -= PlayButtonUp;
        Block.OnFall -= PlayFall;
        PlayerMovement.OnRestart -= PlayRestart;
        Portal.OnPortal -= PlayPortal;
    }

    void PlayJump()
    {
        jump.Play();
    }

    void PlayButtonDown()
    {
        buttonDown.Play();
    }

    void PlayButtonUp()
    {
        buttonUp.Play();
    }

    void PlayFall()
    {
        fall.Play();
    }

    void PlayRestart()
    {
        restart.Play();
        deaths++;
    }

    void PlayPortal()
    {
        portal.Play();
    }
}
