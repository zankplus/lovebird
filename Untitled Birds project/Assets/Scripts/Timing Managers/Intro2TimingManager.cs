using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro2TimingManager : BasicTimingManager {

    // Use this for initialization
    void Start()
    {
        Initialize();

        AddEvent(SweepCameraLeft, 0);
        AddEvent(StartMovingLeft, 0.9f);
        AddEvent(StopMoving, 1f);
        AddEvent(StartMovingRight, 0.6f);
        AddEvent(StopMoving, 0.5f);
        AddEvent(StartMovingLeft, 0.5f);
        AddEvent(StopMoving, 0.25f);
        AddEvent(LookUp, 0.85f);
        AddEvent(PanCameraUp, 0.75f);
        AddEvent(PanCameraDown, 2.5f);
        AddEvent(StopLookingUp, 1.4f);

        AddEvent(ShowLeftBubble, 0.65f);
        AddEvent(ShowExclamationEmojiLeft, 0);
        AddEvent(ShowRightBubble, 0.75f);
        AddEvent(ShowHatEmojiRight, 0);
        AddEvent(ClearLeftBubble, 1.3f);
        AddEvent(ClearRightBubble, 0.3f);

        AddEvent(Jump, 0.2f);
        AddEvent(Jump, 0.3f);
        AddEvent(AllowPlayerMovement, 0.35f);
        BuildQueue();
    }

    void LookUp()
    {
        player.anim.SetBool("LookUp", true);
    }

    void StopLookingUp()
    {
        player.anim.SetBool("LookUp", false);
    }
    
    public void PanCameraUp()
    {
        _camera.currentSpeed = 16;
        _camera.target = new Vector3(_camera.transform.position.x, _camera.transform.position.y + 20 + 0.0001f, _camera.transform.position.z);
        _camera.lerpMode = false;
    }

    public void PanCameraDown()
    {
        _camera.currentSpeed = _camera.fastSpeed;
        _camera.target = new Vector3(_camera.transform.position.x, _camera.transform.position.y - 20 + 0.0001f, _camera.transform.position.z);
        _camera.lerpMode = false;
    }

    void ShowExclamationEmojiLeft()
    {
        ShowEmoji(emoji1, false);
    }

    void ShowHatEmojiRight()
    {
        ShowEmoji(emoji2, true);
    }
}
