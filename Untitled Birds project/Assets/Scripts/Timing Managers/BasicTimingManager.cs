using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains a variety of broadly useful functions for moving the lovebird and animating the scene
public abstract class BasicTimingManager : TimingManager
{
    public Animator doorAnimator;
    public PlayerController player;
    public CameraController _camera;
    public GameObject blinders;
    public FadeIn fade;
    public SpriteRenderer emoji1;
    public SpriteRenderer emoji2;
    

    private SpriteRenderer leftEmoji;
    private SpriteRenderer rightEmoji;

    public void OpenDoor()
    {
        doorAnimator.SetBool("OpenDoor", true);
    }

    public void StartMovingRight()
    {
        player.autoMoveLeft = false;
        player.autoMoveRight = true;
    }

    public void StartMovingLeft()
    {
        player.autoMoveLeft = true;
        player.autoMoveRight = false;
    }

    public void StopMoving()
    {
        player.autoMoveRight = false;
        player.autoMoveLeft = false;
    }

    public void FaceLeft()
    {
        Vector3 scale = player.transform.localScale;
        player.transform.localScale = new Vector3(-1, scale.y, scale.z);

        player.leftBubble.Update();
        player.rightBubble.Update();
    }

    public void FaceRight()
    {
        Vector3 scale = player.transform.localScale;
        player.transform.localScale = new Vector3(1, scale.y, scale.z);

        player.leftBubble.Update();
        player.rightBubble.Update();
    }

    public void ShowLeftBubble()
    {
        player.leftBubble.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void ShowRightBubble()
    {
        player.rightBubble.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void ShowEmoji(SpriteRenderer emoji, bool rightSide)
    {
        if (rightSide)
        {
            rightEmoji = emoji;
            rightEmoji.transform.parent = player.rightBubble.transform;
            emoji.transform.position = player.rightBubble.transform.position;
        }
        else
        {
            leftEmoji = emoji;
            leftEmoji.transform.parent = player.leftBubble.transform;
            emoji.transform.position = player.leftBubble.transform.position;
        }
        
        emoji.enabled = true;
    }

    public void ClearLeftBubble()
    {
        leftEmoji.enabled = false;
        leftEmoji.transform.parent = null;
        leftEmoji = null;
        player.leftBubble.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void ClearRightBubble()
    {
        rightEmoji.enabled = false;
        rightEmoji.transform.parent = null;
        rightEmoji = null;
        player.rightBubble.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SweepCameraRight()
    {
        _camera.target = new Vector3(_camera.transform.position.x + 20 + 0.0001f, _camera.transform.position.y, _camera.transform.position.z);
        _camera.lerpMode = true;
    }

    public void SweepCameraLeft()
    {
        _camera.target = new Vector3(_camera.transform.position.x - 20 + 0.0001f, _camera.transform.position.y, _camera.transform.position.z);
        _camera.lerpMode = true;
    }

    public void Jump()
    {
        player.InitiateJump();
    }

    public void AllowPlayerMovement()
    {
        player.canMove = true;
    }
}
