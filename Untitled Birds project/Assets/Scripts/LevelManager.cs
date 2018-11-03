using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public LevelState state;
    private PlayerController player;
    private CameraController _camera;
    public bool gameOver;
    public ResetOnRespawn[] objectsToReset;

    public GameObject gameOverScreen;
    
    public AudioSource levelMusic;
    public AudioSource gameOverMusic;
    public bool respawnCoActive;
    public bool playerCanMoveImmediately;

    public float cameraPanUpTime;
    public float cameraPanUpSpeed;
    private float cameraPanTimer;

    public enum LevelState { START_LEVEL, IN_PROGRESS, END_LEVEL }

    // Use this for initialization
    void Start ()
    {
        // Level Start
        player = FindObjectOfType<PlayerController>();
        _camera = FindObjectOfType<CameraController>();
        objectsToReset = FindObjectsOfType<ResetOnRespawn>();

        _camera.currentSpeed = _camera.defaultSpeed;
        _camera.target = _camera.lowestPosition.position;
        state = LevelState.START_LEVEL;
    }
	
	// Update is called once per frame
	void Update ()
    {

    }


    public void GameOver()
    {
        gameOver = true;
        player.canMove = false;
        gameOverScreen.SetActive(true);
        FindObjectOfType<RisingWave>().gameObject.SetActive(false);
        TextboxManager[] managers = FindObjectsOfType<TextboxManager>();
        foreach (TextboxManager manager in managers)
        {
            manager.gameObject.SetActive(false);
            manager.voiceBlurble.Stop();
        }

        StartCoroutine("QuitGame");
    }

    private IEnumerator QuitGame()
    {
        Debug.Log("begin yield | " + this);
        yield return new WaitForSeconds(5000f);
        Debug.Log("end yield");
        Application.Quit();
        SceneManager.LoadScene("Demo Level");


    }

}
