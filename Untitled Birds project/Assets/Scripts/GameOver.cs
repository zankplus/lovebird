using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public string levelSelect;
    public string mainMenu;
    private LevelManager levelManager;

	// Use this for initialization
	void Start ()
    {
        // levelManager = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RestartLevel()
    {
      
    }

    public void LevelSelect()
    {
       
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
}
