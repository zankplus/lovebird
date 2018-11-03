using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    Transform player;
    public int direction;   // -1 if left, 1 if right

	// Use this for initialization
	void Start ()
    {
        player = transform.parent;
	}

    // Update is called once per frame
    public void Update()
    {
        // Left bubble
        if (direction == -1)
        { 
            if (player.localScale.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), 1, 1);
                transform.localPosition = new Vector3(-Mathf.Abs(transform.localPosition.x), 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), 1, 1);
                transform.localPosition = new Vector3(Mathf.Abs(transform.localPosition.x), 1, 1);
            }
        }

        // Right bubble
        else
        {
            if (player.localScale.x > 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), 1, 1);
                transform.localPosition = new Vector3(Mathf.Abs(transform.localPosition.x), 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), 1, 1);
                transform.localPosition = new Vector3(-Mathf.Abs(transform.localPosition.x), 1, 1);
            }
        }
    }
}
