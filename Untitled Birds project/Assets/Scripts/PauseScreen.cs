using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour {

    private LevelManager levelManager;
    public string levelSelect;
    public string mainMenu;

    public GameObject pauseScreen;
    private PlayerController player;

	// Use this for initialization
	void Start ()
    {
        levelManager = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetButtonDown("Pause"))
        {
            if (Time.timeScale == 0f)
                ResumeGame();
            else
                PauseGame();
        }
	}

    public void PauseGame()
    {
        Time.timeScale = 0;

        pauseScreen.SetActive(true);
        player.canMove = false;
        levelManager.levelMusic.Pause();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;

        pauseScreen.SetActive(false);
        player.canMove = true;
        levelManager.levelMusic.Play();
    }

    public void LevelSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSelect);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
}
