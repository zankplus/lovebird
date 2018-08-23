using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPatternScroll : MonoBehaviour
{
    public float speed;
    public float upperBound;
    public float resetAmount;
    
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float newY = transform.localPosition.y + speed * Time.deltaTime;

        if (newY > upperBound)
            newY -= resetAmount;

        transform.localPosition = new Vector3(transform.localPosition.z, newY, transform.localPosition.z);
	}
}
