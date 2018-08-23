using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    [Multiline] public string text;
    public int width;
    public int height;
    public float delayAfterMessage;
    public bool done;

    public Transform speechBubble;
    public Transform letters;

    private TextboxManager manager;
    private int maxLineLength;
    private int maxLineCount;
    private int currentLine = 0;
    private int currentColumn = 0;

    private int tailportSide = 0;

    private PlayerController player;
    private Vector2 playerPrevPosition;

    private SpriteRenderer[,] edgeSpriteHolders;

    // Use this for initialization
    void Start()
    {
        manager = GetComponentInParent<TextboxManager>();
        player = FindObjectOfType<PlayerController>();

        transform.localPosition = new Vector3(-width - 1, height / 2, 0f);

        SpriteRenderer[,] boxSpriteHolders = new SpriteRenderer[width, height];
        edgeSpriteHolders = new SpriteRenderer[width, height];

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                for (int k = 0; k < 2; k++)
                {
                    SpriteRenderer spriteRenderer = Instantiate(manager.spriteRendererPrefab);
                    spriteRenderer.transform.localPosition = transform.position + new Vector3(i, -j, 0);
                    spriteRenderer.transform.SetParent(speechBubble);

                    Sprite[] spriteList;

                    // Box
                    if (k == 0)
                    {
                        spriteRenderer.sortingLayerName = "Textbox";
                        spriteList = manager.boxSprites;
                        boxSpriteHolders[i, j] = spriteRenderer;
                    }

                    // Edge
                    else
                    {
                        spriteRenderer.sortingLayerName = "Textbox Edge";
                        spriteList = manager.edgeSprites;
                        edgeSpriteHolders[i, j] = spriteRenderer;
                    }

                    // Set sprite
                    if (i == 0)
                    {
                        if (height == 1)
                            spriteRenderer.sprite = spriteList[9];
                        else if (j == 0)
                            spriteRenderer.sprite = spriteList[0];
                        else if (j == height - 1)
                            spriteRenderer.sprite = spriteList[6];
                        else
                            spriteRenderer.sprite = spriteList[3];
                    }
                    else if (i == width - 1)
                    {
                        if (height == 1)
                            spriteRenderer.sprite = spriteList[11];
                        else if (j == 0)
                            spriteRenderer.sprite = spriteList[2];
                        else if (j == height - 1)
                            spriteRenderer.sprite = spriteList[8];
                        else
                            spriteRenderer.sprite = spriteList[5];
                    }
                    else
                    {
                        if (height == 1)
                            spriteRenderer.sprite = spriteList[10];
                        else if (j == 0)
                            spriteRenderer.sprite = spriteList[1];
                        else if (j == height - 1)
                            spriteRenderer.sprite = spriteList[7];
                        else
                            spriteRenderer.sprite = spriteList[4];
                    }
                }

        // Add tailport
        if (tailportSide == 0)
        {
            if (height == 1)
                edgeSpriteHolders[width - 1, 0].sprite = manager.tailports[0];
            else
            {
                int tailportHeight = height / 2;

                if (tailportHeight == 0)
                    edgeSpriteHolders[width - 1, tailportHeight].sprite = manager.tailports[1];
                else if (tailportHeight == height - 1)
                    edgeSpriteHolders[width - 1, tailportHeight].sprite = manager.tailports[3];
                else
                    edgeSpriteHolders[width - 1, tailportHeight].sprite = manager.tailports[2];
            }

            manager.tail.transform.parent = transform;
            manager.tail.transform.localPosition = new Vector3(width, -height / 2, transform.position.z);
            manager.tail.SetActive(true);
        }

        maxLineCount = 3 * height - 2;
        maxLineLength = 4 * width - 2;


        StartCoroutine("DisplayText");
        playerPrevPosition = player.transform.position;
        SetMessageBoxSide();
    }

    private void Update()
    {

        if (playerPrevPosition.x != player.transform.position.x)
        {
            SetMessageBoxSide();
        }

        playerPrevPosition = player.transform.position;
    }

    private IEnumerator DisplayText()
    {
        string[] tokens = text.Split(' ');

        manager.voiceBlurble.Play();

        for (int i = 0; i < tokens.Length; i++)
        {
            string token = tokens[i];

            // Print word
            if (currentColumn + GetFunctionalLength(token) > maxLineLength)
            {
                currentColumn = 0;
                currentLine++;
            }

            if (currentLine < maxLineCount)
            {
                //Print character
                char[] chars = token.ToCharArray();
                for (int j = 0; j < token.Length; j++)
                {
                    yield return new WaitForSeconds(manager.textSpeed);
                    int x = manager.charMap[chars[j]];

                    if (x >= 0)
                    {
                        SpriteRenderer character = Instantiate(manager.spriteRendererPrefab, letters);
                        character.sprite = manager.fontSprites[x];
                        character.sortingLayerName = "Text";
                        character.transform.parent = letters;
                        character.transform.localPosition = new Vector3(currentColumn * 0.25f - 0.15f, -currentLine * 0.375f + 0.06f, character.transform.position.z);
                        currentColumn++;
                    }
                    else if (x == -1)
                    {
                        currentColumn = 0;
                        currentLine++;
                    }
                    else if (x == -2)
                    {
                        manager.voiceBlurble.Pause();
                        yield return new WaitForSeconds(manager.delayLength);
                        manager.voiceBlurble.UnPause();
                    }
                }

                if (currentColumn > 0)
                    currentColumn++;
            }
        }

        manager.voiceBlurble.Stop();

        yield return new WaitForSeconds(manager.baseDelayBeforeClosing + 0.015f * text.Length);
        manager.timeTilNextBox = delayAfterMessage;
        done = true;
        gameObject.SetActive(false);
    }

    private void SetMessageBoxSide()
    {
        // Player right, message left
        if (player.transform.position.x > manager.transform.position.x + 0.5f)
        {
            transform.localPosition = new Vector3(-Mathf.Abs(transform.localPosition.x), transform.localPosition.y, transform.localPosition.z);
            transform.localScale = Vector3.one;
            letters.localPosition = Vector3.zero;
            letters.localScale = Vector3.one;
            manager.tail.transform.localScale = Vector3.one;
        }

        // Player left, message right
        else if (player.transform.position.x < manager.transform.position.x - 0.5f)
        {
            transform.localPosition = new Vector3(Mathf.Abs(transform.localPosition.x), transform.localPosition.y, transform.localPosition.z);
            transform.localScale = new Vector3(-1, 1, 1);
            letters.localPosition = new Vector3(width - 1f, 0, 0);
            letters.localScale = new Vector3(-1, 1, 1);
            manager.tail.transform.localScale = Vector3.one;
        }
    }

    private int GetFunctionalLength(string str)
    {
        int count = 0;

        char[] chars = str.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
            if (chars[i] >= 0)
                count++;

        return count;
    }
}
