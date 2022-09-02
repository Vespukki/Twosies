using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    SpriteRenderer spriter;
    [SerializeField] List<int> layers;
    [SerializeField] float pushForce = 2;

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        foreach(var hit in Physics2D.BoxCastAll(transform.position - transform.up * spriter.size.x, new Vector2(spriter.size.x, spriter.size.x), transform.position.z ,-transform.up))
        {
            if (layers.Contains(hit.collider.gameObject.layer))
            {
                if(hit.collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D body))
                {
                    body.AddForce(-transform.up * pushForce);
                }
                spriter.size = new Vector2(spriter.size.x, hit.distance + .8625f);
                return;
            }
        }
        spriter.size = new Vector2(spriter.size.x, 30);
    }
}
