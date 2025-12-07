/* 
Torpedo.cs
By: Jake Schott
*/

using UnityEngine;

public class Torpedo : MonoBehaviour
{
    private static float TORPEDO_SPEED = 50.0f;

    void Update()
    {
        transform.GetChild(0).LookAt(Camera.main.transform.position);   
        transform.position += transform.forward * Time.deltaTime * TORPEDO_SPEED;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Asteroid")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
