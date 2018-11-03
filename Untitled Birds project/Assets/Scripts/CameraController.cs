using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float currentSpeed;
    public float defaultSpeed;
    public float fastSpeed;
    public bool focusUpward;
    public Transform upperBound;
    public Transform lowerBound;
    public Transform lowestPosition;
    public Transform highestPosition;
    public Transform downwardAnchorReversalPoint;
    public Transform upwardAnchorReversalPoint;
    public Transform downwardAnchor;
    public Transform upwardAnchor;
    public Transform fallingFollowTarget;
    public Vector3 target;

    public bool lerpMode;
    public float cameraLerpTimer;
    public float cameraLerpTime;
    public float cameraLerpSpeed;

    private float increment;
    private Camera _camera;
    private PlayerController player;

    // Use this for initialization
    void Start ()
    {
        _camera = GetComponentInParent<Camera>();
        _camera.orthographicSize = Screen.width / (2f * 24f * 2f * Screen.width / Screen.height);
        increment = 1 / 24f;

        player = FindObjectOfType<PlayerController>();

        focusUpward = true;
	}

    // Update is called once per frame
    void LateUpdate()
    {
        if (lerpMode)
        {
            transform.position = Vector3.Lerp(transform.position, target, cameraLerpTimer * cameraLerpSpeed);

            cameraLerpTimer += Time.deltaTime;

            if (transform.position == target)
            {
                lerpMode = false;
                cameraLerpTimer = 0;
                Debug.Log("Done lerping to " + target);
            }
        }
        else
        {
            // If the level is in progress, the camera should follow the player
            if (focusUpward)
            {
                if (player.isGrounded && player.rb.velocity.y == 0)
                {
                    // Use platform snapping for upward movement
                    if (player.transform.position.y > target.y)
                    {

                        target = new Vector3(transform.position.x, player.transform.position.y - upwardAnchor.localPosition.y, transform.position.z);

                    }
                }

                //else if (player.transform.position.y > downwardAnchor.position.y)
                //   target = new Vector3(transform.position.x, player.transform.position.y - upwardAnchor.localPosition.y, transform.position.z);

                else if (player.transform.position.y < downwardAnchorReversalPoint.position.y && player.rb.velocity.y < 0)
                    focusUpward = false;
            }
            else
            {
                if (player.transform.position.y < downwardAnchor.position.y)
                    target = new Vector3(transform.position.x, player.transform.position.y - downwardAnchor.localPosition.y, transform.position.z);

                if (player.transform.position.y > upwardAnchorReversalPoint.position.y && player.rb.velocity.y > 0)
                    focusUpward = true;
            }

            // Move toward target
            if (target.y > transform.position.y)
            {
                transform.Translate(Vector3.up * Mathf.Max(currentSpeed, Mathf.Abs(player.rb.velocity.y)) * Time.deltaTime);

                if (transform.position.y > target.y)
                    transform.position = new Vector3(transform.position.x, target.y, transform.position.z);
            }
            else if (target.y < transform.position.y)
            {
                transform.Translate(Vector3.down * Mathf.Max(currentSpeed, Mathf.Abs(player.rb.velocity.y) + currentSpeed) * Time.deltaTime);

                if (transform.position.y < target.y)
                    transform.position = new Vector3(transform.position.x, target.y, transform.position.z);
            }

            // Don't move below the lowest point of the level
            if (transform.position.y < lowestPosition.position.y)
                transform.position = lowestPosition.position;

            // Or above the highest
            if (transform.position.y > highestPosition.position.y)
                transform.position = highestPosition.position;

            // Snap camera to pixel grid
            transform.position = new Vector3(transform.position.x - mod(transform.position.x, increment), transform.position.y - mod(transform.position.y, increment), transform.position.z);
        }
    }

    private float mod(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(target, 0.25f * Vector3.one);
    }
}

