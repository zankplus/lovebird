using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void RecedeIntoBackground()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "World Objects";
    }

    void AdvanceIntoForeground()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "Foreground";
    }
}
