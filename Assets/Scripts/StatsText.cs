using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsText : MonoBehaviour
{
    [SerializeField] TextMeshPro tmpro;
   void Start()
    {

        Debug.Log(Time.time);
        tmpro.SetText(
            "Time: " + FormatTime() + "\n" +
            "Deaths: " + MusicManager.Instance.deaths);

    }

    string FormatTime()
    {
        float seconds = Time.time;
        float hours = 0;
        float minutes = 0;

        while(seconds > 6000)
        {
            hours++;
            seconds -= 6000;
        }
        while(seconds > 60)
        {
            minutes++;
            seconds -= 60;
        }
        return hours + ":" + minutes + ":" + seconds;
    }
}
