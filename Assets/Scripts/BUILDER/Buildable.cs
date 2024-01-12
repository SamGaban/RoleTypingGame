using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script attached to an object that is placeable in town by the player
/// <para>Do not forget to tag the gameObject as saveable
/// <para>Do no forget to also add it to build dictionary
/// </summary>
public class Buildable : MonoBehaviour
{
    /// <summary>
    /// Represents a game session.
    /// </summary>
    private GameSession session;

    /// <summary>
    /// Represents the current editing state.
    /// </summary>
    private bool editing = false;

    /// <summary>
    /// The sensitivity of the building feature
    /// </summary>
    private float sensitivity = 1.0f;

    /// <summary>
    /// The original color of the buildable to call back after editing it
    /// </summary>
    private Color originalColor;

    private float coolDownClick = 0f;

    public int buildIndex;

    public int goldValue;

    public enum Category
    {
        Buildings,
        Utility,
        Furniture,
        Fire
    }

    public Category category;



    private void Start()
    {
        session = FindObjectOfType<GameSession>();
        originalColor = this.GetComponent<SpriteRenderer>().color;
    }

    /// <summary>
    /// Highlights item on mouse over
    /// </summary>
    private void OnMouseOver()
    {
        if (editing) return;

        if (session.inEditMode) return;

        if (session.buildPanelOpen) return;
        
        
        this.GetComponent<SpriteRenderer>().color = Color.magenta;
    }

    
    /// <summary>
    /// Gives item its color back
    /// </summary>
    private void OnMouseExit()
    {
        if (editing) return;
        
        this.GetComponent<SpriteRenderer>().color = originalColor;
    }


    /// <summary>
    /// Updates the position of the buildable game object based on mouse input.
    /// </summary>
    private void Update()
    {
        if (!editing) return;
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        this.gameObject.transform.position = Vector2.Lerp(transform.position, mousePosition, sensitivity * Time.deltaTime);

        if (Input.GetMouseButtonDown(0) && Time.time - coolDownClick >= 0.3f)
        {
            session.DeactivateEditMove();
            editing = false;
        }
    }

    /// <summary>
    /// Sets the editing variable to true
    /// </summary>
    public void SetEditingTrue()
    {
        editing = true;
    }
    
    /// <summary>
    /// If item is clicked, go in build mode with it as item
    /// <para>make the item the edited item in the GameSession + edited item index
    /// </summary>
    private void OnMouseDown()
    {
        if (!editing)
        {
            if (session.inEditMode) return;

            if (session.buildPanelOpen) return;

            session.SetAsEditedObject(this.gameObject, buildIndex);

            this.GetComponent<SpriteRenderer>().color = originalColor;

            session.ActivateEditMove(this.gameObject);
            editing = true;

            coolDownClick = Time.time;

        }
        
    }

}
