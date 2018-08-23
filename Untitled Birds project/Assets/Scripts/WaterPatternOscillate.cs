using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPatternOscillate : MonoBehaviour
{
    public float xOffset, yOffset;
    public float horizontalAmplitude;
    public float horizontalFrequency;
    public float verticalAmplitude;
    public float verticalFrequency;

    public float time;

    // Use this for initialization
    void Start ()
    {
        time = Mathf.PI * 0.5f;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        time += Time.deltaTime;

        float newX = xOffset + Mathf.Sin(time * horizontalFrequency) * horizontalAmplitude;
        float newY = yOffset + Mathf.Sin(time * verticalFrequency) * verticalAmplitude;

        transform.localPosition = new Vector3(newX, newY, transform.localPosition.z);
    }
}
