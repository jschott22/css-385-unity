/* 
BeaconFlasher.cs
By: Jake Schott
*/

using System.Collections;
using UnityEngine;

public class BeaconFlasher : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(infiniteFlasher());
    }

    IEnumerator infiniteFlasher()
    {
        while (true)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
