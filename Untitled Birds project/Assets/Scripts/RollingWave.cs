using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingWave : MonoBehaviour
{
    public Transform source;
    public Transform destination;
    public float speed;
    // public RollingWave nextWave;
    public float waveLength;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x < destination.position.x)
        {
            transform.position = new Vector3(transform.position.x + (source.position.x - destination.position.x), transform.position.y, transform.position.z);
        }
	}
}
