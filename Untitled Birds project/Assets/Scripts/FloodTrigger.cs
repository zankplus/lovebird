using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodTrigger : MonoBehaviour
{
    public float speed;
    private RisingWave risingWave;

	// Use this for initialization
	void Start ()
    {
        risingWave = FindObjectOfType<RisingWave>();
	}
	
	private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player" && transform.position.y > risingWave.target.position.y)
        {
            risingWave.speed = speed;
            risingWave.target = transform;
        }
	}
}
