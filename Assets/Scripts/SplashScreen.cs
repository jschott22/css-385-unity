/*
SplashScreen.cs
By: Jake Schott
*/

using System.Collections;
using TMPro;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public UnityEngine.UI.RawImage cover_up;
    public TMP_Text title;
    public TMP_Text title_shadow;
    public TMP_Text any_button;

    private void Start()
    {
        StartCoroutine(hideCoverup());
    }

    IEnumerator hideCoverup()
    {
        yield return new WaitForSeconds(1.0f);

        float anim_time = 1.0f;
        while (anim_time > 0.0f)
        {
            anim_time = Mathf.Max(0.0f, anim_time - Time.deltaTime);
            cover_up.color = new Color(0.0f, 0.0f, 0.0f, anim_time);
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        anim_time = 1.0f;
        while (anim_time > 0.0f)
        {
            anim_time = Mathf.Max(0.0f, anim_time - Time.deltaTime);
            title.color = new Color(title.color.r, title.color.g, title.color.b, 1.0f - anim_time);
            title_shadow.color = new Color(title_shadow.color.r, title_shadow.color.g, title_shadow.color.b, 1.0f - anim_time);
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        anim_time = 1.0f;
        while (anim_time > 0.0f)
        {
            anim_time = Mathf.Max(0.0f, anim_time - Time.deltaTime);
            any_button.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - anim_time);
            yield return null;
        }
    }
}