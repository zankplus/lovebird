using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public float fadeTime;
    public bool fadeIn;
    public bool fadeOut;
    private Image blackScreen;

	// Use this for initialization
	void Start ()
    {
        blackScreen = GetComponent<Image>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (fadeIn)
        {
            blackScreen.CrossFadeAlpha(0f, fadeTime, false);
        }
        else if (fadeOut)
        {
            blackScreen.CrossFadeAlpha(1f, fadeTime, false);
        }
    }
}
