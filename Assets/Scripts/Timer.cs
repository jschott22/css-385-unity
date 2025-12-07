/* 
Timer.cs
By: Jake Schott
*/

using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private int time_left = 120;

    void Start()
    {
        StartCoroutine(countdown());        
    }

    IEnumerator countdown()
    {
        while (time_left > 0)
        {
            yield return new WaitForSeconds(1.0f);
            time_left -= 1;
            string time_string = (time_left / 60) + ":" + (time_left % 60);
            if (time_left % 60 < 10)
            {
                time_string = (time_left / 60) + ":0" + (time_left % 60);
            }
            transform.GetComponent<TMP_Text>().SetText(time_string);
        }
    }
}
