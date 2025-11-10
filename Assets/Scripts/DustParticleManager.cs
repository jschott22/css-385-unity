/* 
DustParticleManager.cs
By: Jake Schott
*/

using System.Collections;
using UnityEngine;

public class DustParticleManager : MonoBehaviour
{
    //CLASS CONSTANTS
    private static int NUM_PARTICLES = 500;
    private static float MIN_DISTANCE = 40.0f;
    private static float MAX_DISTANCE = 155.0f;
    private static float STAR_MIN_SIZE = 0.35f;
    private static float STAR_MAX_SIZE = 0.55f;
    private static float TIME_TO_APPEAR = 1.0f; //seconds

    private GameObject spaceship;

    IEnumerator resetStar(GameObject star)
    {
        yield return resizeStar(star, 0.0f);
        float star_size = Random.Range(STAR_MIN_SIZE, STAR_MAX_SIZE);
        star.transform.position = spaceship.transform.position + Random.insideUnitSphere * MAX_DISTANCE * Random.Range(0.7f, 1.0f);
        StartCoroutine(resizeStar(star, star_size));
    }

    //used to grow the particle so it doesn't come out of nowhere
    IEnumerator resizeStar(GameObject star, float desired_size)
    {
        float grow_time = TIME_TO_APPEAR;
        float start_size = star.transform.localScale.x;
        while (grow_time > 0.0f)
        {
            grow_time -= Time.deltaTime;
            float current_size = Mathf.Lerp(desired_size, start_size, (grow_time / TIME_TO_APPEAR));
            star.transform.localScale = new Vector3(current_size, current_size, 1.0f);
            yield return null;
        }
        star.transform.localScale = new Vector3(desired_size, desired_size, 0.0f);
    }

    private void Start()
    {
        //get ship
        spaceship = GameObject.FindGameObjectWithTag("Ship");

        //initialize particles
        for (int i = 0; i < NUM_PARTICLES; i++)
        {
            GameObject new_star = GameObject.Instantiate(transform.GetChild(0).gameObject, transform);
            float star_size = Random.Range(STAR_MIN_SIZE, STAR_MAX_SIZE);
            new_star.transform.localScale = new Vector3(star_size, star_size, star_size);
            new_star.transform.position = spaceship.transform.position + Random.insideUnitSphere * (MAX_DISTANCE - MIN_DISTANCE) * Random.Range(0.3f, 1.0f);
            new_star.SetActive(true);
        }
    }
    private void LateUpdate()
    {
        //face particles towards camera
        //if greater than max distance, replace particle
        if (Camera.main == null)
        {
            return;
        }
        if (spaceship == null)
        {
            GameObject.Destroy(gameObject);
            return;
        }
        for (int i = 1; i <= NUM_PARTICLES; i++)
        {
            transform.GetChild(i).LookAt(Camera.main.transform.position);
            float dist = Vector3.Distance(transform.GetChild(i).position, spaceship.transform.position);
            if (dist > MAX_DISTANCE)
            {
                if (transform.GetChild(i).localScale.z < 1.0f)
                {
                    StartCoroutine(resetStar(transform.GetChild(i).gameObject));
                }
            }
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp((dist - MIN_DISTANCE) / MIN_DISTANCE, 0.0f, 1.0f));
        }
    }
}
