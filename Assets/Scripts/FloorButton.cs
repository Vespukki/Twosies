using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorButton : MonoBehaviour
{
    [SerializeField] List<Tilemap> toggles = new List<Tilemap>();
    [SerializeField] Color clearColor;

    bool isPressed;
    bool pressable = true;
    List<GameObject> pressers = new List<GameObject>();

    Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Press(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Unpress(collision);
    }

    void Press(Collider2D collision)
    {
        if (!pressable) return;
        if (collision.CompareTag("Player") || collision.gameObject.layer == 11 || collision.gameObject.layer == 12)
        {
            pressers.Add(collision.gameObject);
            if (!isPressed)
            {
                isPressed = true;
                animator.SetBool("Pressed", isPressed);
            }
        }
    }

    void Unpress(Collider2D collision)
    {
        if (pressers.Contains(collision.gameObject))
        {
            pressers.Remove(collision.gameObject);
            if (pressers.Count == 0)
            {
                isPressed = false;
                animator.SetBool("Pressed", isPressed);
                pressable = false;
            }
        }
    }

    public void ButtonUp()
    {
        Toggle();
        pressable = true;
    }
    public void ButtonDown()
    {
        Toggle();
    }

    void Toggle()
    {
        foreach (var tilemap in toggles)
        {
            if (tilemap.gameObject.GetComponent<TilemapCollider2D>().enabled == true)
            {
                tilemap.gameObject.GetComponent<TilemapCollider2D>().enabled = false;
                tilemap.color = clearColor;
            }
            else
            {
                tilemap.gameObject.GetComponent<TilemapCollider2D>().enabled = true;
                tilemap.color = Color.white;
            }
        }
    }
}
