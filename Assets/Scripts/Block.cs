using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    Rigidbody2D body;

    public delegate void SoundDelegate();
    public static event SoundDelegate OnFall;

    float yVelocityLastFrame = 0;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        yVelocityLastFrame = body.velocity.y;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (yVelocityLastFrame < -3f)
        {
            if(OnFall != null)
            {
                OnFall();
            }
        }
    }
}
