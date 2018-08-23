using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource levelTheme;
    public AudioSource drowningTheme;
    public AudioSource current;

    public float fadeInSpeed;
    public float fadeOutSpeed;

    public bool fadeOut;
    public bool fadeIn;
    
    public bool queueDrowningTheme;
    public float drowningThemeWait;
    public float queueDrowningThemeCounter;

	// Use this for initialization
	void Start ()
    {
        current = levelTheme;
        DontDestroyOnLoad(transform);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (fadeOut)
        {
            current.SetLoudness(current.GetLoudness() - Time.deltaTime * fadeOutSpeed);
            if (current.GetLoudness() <= 0)
            {
                current.SetLoudness(0f);
                current.Pause();
                fadeOut = false;
            }
        }

        else if (fadeIn)
        {
            current.SetLoudness(current.GetLoudness() + Time.deltaTime * fadeInSpeed);
            if (current.GetLoudness() >= 1)
            {
                current.SetLoudness(1f);
                fadeIn = false;
            }
        }

        if (queueDrowningTheme)
        {
            queueDrowningThemeCounter -= Time.deltaTime;

            if (queueDrowningThemeCounter < 0)
            {
                queueDrowningTheme = false;
                drowningTheme.Play();
            }
        }

    }

    public void Submerge()
    {
        fadeOut = true;
        fadeIn = false;

        queueDrowningTheme = true;
        queueDrowningThemeCounter = drowningThemeWait;
    }
    
    public void Emerge()
    {
        drowningTheme.Stop();

        queueDrowningTheme = false;

        fadeIn = true;
        fadeOut = false;
        current = levelTheme;
        current.UnPause();
        Debug.Log(levelTheme.time);
    }
}
