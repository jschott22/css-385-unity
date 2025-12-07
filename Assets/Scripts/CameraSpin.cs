/* 
CameraSpin.cs
By: Jake Schott
*/

using UnityEngine;

public class CameraSpin : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0.2f * Time.deltaTime, 0.0f);
    }
}