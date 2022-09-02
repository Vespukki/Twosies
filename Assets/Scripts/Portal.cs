using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] int nextLevel;

    public delegate void SoundDelegate();
    public static event SoundDelegate OnPortal;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().teleported = true;
            if(OnPortal != null)
            {
                OnPortal();
            }
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        AsyncOperation progress = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while (!progress.isDone)
        {
            yield return null;
        }
    }
}
