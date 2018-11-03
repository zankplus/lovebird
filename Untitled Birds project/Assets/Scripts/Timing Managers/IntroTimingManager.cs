using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroTimingManager : BasicTimingManager
{
    public Animator exitAnimator;

    // Use this for initialization
	void Start ()
    {
        Initialize();

        AddEvent(SweepCameraRight, 1);
        AddEvent(OpenDoor, 2f);
        AddEvent(StartMovingRight, 1.25f);
        AddEvent(StopMoving, .85f);
        AddEvent(FaceLeft, 0.8f);
        AddEvent(FaceRight, 0.5f);
        AddEvent(FaceLeft, 0.5f);
        AddEvent(ShowLeftBubble, 1);
        AddEvent(ShowHatEmojiLeft, 0);
        AddEvent(FaceRight, 0.5f);
        AddEvent(ShowRightBubble, 0.5f);
        AddEvent(ShowQuestionEmojiRight, 0);
        AddEvent(ClearLeftBubble, 1.75f);
        AddEvent(ClearRightBubble, 0.5f);
        AddEvent(StartMovingRight, 0.5f);
        AddEvent(StopMoving, 1.55f);
        AddEvent(SnapToPosition, 0.2f);
        AddEvent(CloseDoor, 0.3f);
        AddEvent(SweepCameraRight, 1);
        AddEvent(LoadIntro2, 0.75f);

        BuildQueue();
    }

    void ShowHatEmojiLeft()
    {
        ShowEmoji(emoji1, false);
    }

    void ShowQuestionEmojiRight()
    {
        ShowEmoji(emoji2, true);
    }

    void CloseDoor()
    {
        exitAnimator.SetBool("CloseDoor", true);
    }

    void SnapToPosition()
    {
        Debug.Log("Snapping to " + exitAnimator.transform.position);
        player.transform.position = exitAnimator.transform.position;
    }

    void LoadIntro2()
    {
        SceneManager.LoadScene("Intro 2");
    }
}
