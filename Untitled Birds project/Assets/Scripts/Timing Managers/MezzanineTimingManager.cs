using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MezzanineTimingManager : TimingManager
{
    public Animator doorAnimator;

    // Use this for initialization
	void Start ()
    {
        Initialize();
        AddEvent(OpenDoor, 1);
	}
	
    void OpenDoor()
    {
        doorAnimator.SetBool("OpenDoor", true);
    }
}
