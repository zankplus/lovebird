using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMasker : MonoBehaviour
{
    private SpriteMask mask;
    public Sprite frame1;
    public Sprite frame2;
    public Sprite frame3;
    public Sprite frame4;
    public Sprite frame5;
    public Sprite frame6;

    // Use this for initialization
    void Start ()
    {
        mask = GetComponent<SpriteMask>();
	}
	
	void Frame1() { mask.sprite = frame1; }
    void Frame2() { mask.sprite = frame2; }
    void Frame3() { mask.sprite = frame3; }
    void Frame4() { mask.sprite = frame4; }
    void Frame5() { mask.sprite = frame5; }
    void Frame6() { mask.sprite = frame6; }
}
