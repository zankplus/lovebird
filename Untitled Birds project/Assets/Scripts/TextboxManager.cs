using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextboxManager : MonoBehaviour
{
    public float textSpeed = 0.05f;
    public float delayLength = 0.1f;

    public Sprite[] boxSprites;
    public Sprite[] edgeSprites;
    public Sprite[] tailports;
    public Texture2D font;
    public Sprite[] fontSprites;
    public Dictionary<char, int> charMap;
    public float baseDelayBeforeClosing = 1.5f;

    public SpriteRenderer spriteRendererPrefab;
    public GameObject tail;
    public AudioSource voiceBlurble;
    private bool displaying;

    private LevelManager levelManager;

    public TextBox[] textBoxes;
    public int currentBox = 0;

    public float timeTilNextBox;

    // Use this for initialization
    void Start ()
    {
        if (charMap == null)
            DefineCharMap();

        fontSprites = Resources.LoadAll<Sprite>(font.name);
        voiceBlurble = GetComponent<AudioSource>();
        levelManager = FindObjectOfType<LevelManager>();
        textBoxes = GetComponentsInChildren<TextBox>(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!levelManager.gameOver)
        {
            if (displaying && currentBox < textBoxes.Length)
            {
                if (!textBoxes[currentBox].done)
                    textBoxes[currentBox].gameObject.SetActive(true);
                else
                {
                    timeTilNextBox -= Time.deltaTime;
                    if (timeTilNextBox < 0)
                    {
                        currentBox++;
                        if (currentBox >= textBoxes.Length)
                            displaying = false;
                    }
                }
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !displaying)
        {
            StartCoroutine("WaitToDisplay");
        }
            
    }

    private IEnumerator WaitToDisplay()
    {
        yield return new WaitForSeconds(0.75f);
        displaying = true;
    }

    private void DefineCharMap()
    {
        charMap = new Dictionary<char, int>();

        charMap['a'] = 0;
        charMap['b'] = 1;
        charMap['c'] = 2;
        charMap['d'] = 3;
        charMap['e'] = 4;
        charMap['f'] = 5;
        charMap['g'] = 6;
        charMap['h'] = 7;
        charMap['i'] = 8;
        charMap['j'] = 9;
        charMap['k'] = 10;
        charMap['l'] = 11;
        charMap['m'] = 12;
        charMap['n'] = 13;
        charMap['o'] = 14;
        charMap['p'] = 15;
        charMap['q'] = 16;
        charMap['r'] = 17;
        charMap['s'] = 18;
        charMap['t'] = 19;
        charMap['u'] = 20;
        charMap['v'] = 21;
        charMap['w'] = 22;
        charMap['x'] = 23;
        charMap['y'] = 24;
        charMap['z'] = 25;
        charMap['!'] = 26;
        charMap['?'] = 27;
        charMap['\''] = 28;
        charMap['.'] = 29;
        charMap['/'] = -1;
        charMap['*'] = -2;
    }
}
