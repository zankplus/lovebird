using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSButton : MonoBehaviour
{
    public int id;
    public SpriteRenderer[] edgeSprites;
    private BoxCollider2D _collider;
    private ButtonManager buttonManager;

	// Use this for initialization
	void Start ()
    {
        _collider = GetComponent<BoxCollider2D>();
        buttonManager = GetComponentInParent<ButtonManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SelectButton()
    {
        if (buttonManager.canSelect)
        {
            // Deselect previous button
            if (buttonManager.selected >= 0 && buttonManager.selected < buttonManager.buttons.Length)
                buttonManager.buttons[buttonManager.selected].DeselectButton();

            // Change color to orange
            for (int i = 0; i < edgeSprites.Length; i++)
                edgeSprites[i].color = Color.white;

            buttonManager.selected = id;
        }
    }

    public void DeselectButton()
    {
        if (buttonManager.canSelect)
        {
            // Change color to black
            for (int i = 0; i < edgeSprites.Length; i++)
                edgeSprites[i].color = Color.black;

            // Set the selected button null, if this one is presently selected
            if (buttonManager.selected == id)
                buttonManager.selected = -1;
        }
    }

    private void OnMouseEnter()
    {
        SelectButton();
    }

    private void OnMouseExit()
    {
        DeselectButton();
    }

    private void OnMouseUp()
    {
        SelectButton();
        buttonManager.ClickButton();
    }
}
