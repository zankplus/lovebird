using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvelopeController : MonoBehaviour
{
    public float entrySpeed;
    public float waveFallSpeed;

    public float timeBeforeOpening;
    public float timeBeforeLovebirdText;
    public float timeBeforeFlooding;
    public float timeBeforeReceding;
    public float timeBeforeSubmergedText;

    public float lovebirdTextSpeed;
    public float submergedTextSpeed;
    public float waveRisingSpeed;

    public GameObject upperLayer;
    public SpriteRenderer lovebirdText;
    public SpriteRenderer submergedText;
    public SpriteRenderer blueBackground;
    public GameObject risingWave;
    public GameObject[] waterPatterns;
    public ButtonManager buttonManager;
    public GameObject startButton;
    public GameObject quitButton;
    public Transform entryTarget;
    public Transform waveTarget;

    private float t;

    private Animator anim;

    private int phase;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        phase = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Move Envelope into position before opening it
        if (phase == 0)
        {
            transform.Translate(Vector2.right * entrySpeed * Time.deltaTime);
            if (transform.position.x > entryTarget.position.x)
            {
                transform.position = new Vector3(entryTarget.position.x, transform.position.y, transform.position.z);
                phase = -1;
                StartCoroutine("OpenEnvelope");
            }
        }

        // Lovebird comes out of envelope
        else if (phase == 1)
        {
            t += Time.deltaTime;

            if (lovebirdText.transform.position.x > entryTarget.position.x)
            {
                lovebirdText.transform.position = entryTarget.position;
                phase = -1;
                StartCoroutine("WaveRise");
            }
            else
                lovebirdText.transform.position = Vector3.Lerp(lovebirdText.transform.position, entryTarget.position + new Vector3(0.04f, 0, 0), t * lovebirdTextSpeed);
        }

        // Wave Rises
        else if (phase == 2)
        {
            t += Time.deltaTime;

            if (risingWave.transform.position.y > waveTarget.position.y)
            {
                risingWave.transform.position = waveTarget.position;
                phase = -1;
                RevealSubmergeText();
            }
            else
                risingWave.transform.position = Vector3.Lerp(risingWave.transform.position, waveTarget.position + new Vector3(0, 0.04f, 0), t * waveRisingSpeed);
        }

        else if (phase == 3)
        {
            t += Time.deltaTime;

            if (submergedText.transform.position.x > entryTarget.position.x)
            {
                submergedText.transform.position = entryTarget.position;
                phase = -1;
                StartCoroutine("WaveFall");
            }
            else
                submergedText.transform.position = Vector3.Lerp(submergedText.transform.position, entryTarget.position + new Vector3(0.04f, 0, 0), t * submergedTextSpeed - timeBeforeSubmergedText);
        }

        else if (phase == 4)
        {
            t += Time.deltaTime;

            risingWave.transform.Translate(Vector2.down * waveFallSpeed * Time.deltaTime);
            if (risingWave.transform.position.y < waveTarget.position.y - 13f)
            {
                Debug.Log(risingWave.transform.position.y + " < " + (waveTarget.position.y - 12f));
                phase = -1;
                buttonManager.MakeButtonsSelectable();
            }
        }
    }

    private IEnumerator OpenEnvelope()
    {
        yield return new WaitForSeconds(timeBeforeOpening);
        anim.SetBool("Open", true);
        yield return new WaitForSeconds(timeBeforeLovebirdText);
        phase = 1;
        t = 0;
    }

    private IEnumerator WaveRise()
    {
        yield return new WaitForSeconds(timeBeforeFlooding);
        lovebirdText.maskInteraction = SpriteMaskInteraction.None;
        phase = 2;
        t = 0;
    }

    private void RevealSubmergeText()
    {
        phase = 3;
        t = 0;
        waterPatterns[0].SetActive(true);
        waterPatterns[1].SetActive(true);
        blueBackground.gameObject.SetActive(true);
    }

    private IEnumerator WaveFall()
    {
        yield return new WaitForSeconds(timeBeforeReceding);
        phase = 4;
        t = 0;

        startButton.SetActive(true);
        quitButton.SetActive(true);
    }

    private void ShowUpperLayer()
    {
        upperLayer.SetActive(true);
    }
}
