using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorExit : MonoBehaviour
{
    public string sceneToLoad;
    public CameraController _camera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && Input.GetAxisRaw("Vertical") > 0)
        {
            // _camera.cameraState = CameraState.END_LEVEL;
            _camera.target = _camera.highestPosition.position + Vector3.up * 9;
            _camera.currentSpeed = _camera.fastSpeed;

            StartCoroutine("LoadScene");
        }
    }

    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);
    }
}
