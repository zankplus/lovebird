using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public float waitBeforeLoad;
    public string firstScene;
    public LSButton[] buttons;
    public GameObject blackWave;

    public int selected;
    public bool canSelect;

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].id = i;
	}

    private void Update()
    {
        if (canSelect)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                SelectButton(0);  // start button
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                SelectButton(1); // quit button
            }
            else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            {
                ClickButton();
            }
        }
    }

    public void MakeButtonsSelectable()
    {
        canSelect = true;
    }

    public void SelectButton(int i)
    {
        buttons[i].SelectButton();
    }

    public void ClickButton()
    {
        // START button
        if (selected == 0)
        {
            canSelect = false;
            blackWave.SetActive(true);
            StartCoroutine("LoadLevel");
        }
        else if (selected == 1)
        {
            Application.Quit();
        }
    }

    public IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(waitBeforeLoad);
        SceneManager.LoadScene(firstScene);
    }
}
