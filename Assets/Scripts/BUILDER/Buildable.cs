using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour
{
    private GameSession session;

    private bool editing = false;

    private float sensitivity = 1.0f;

    private Color originalColor;

    private float coolDownClick = 0f;



    private void Start()
    {
        session = FindObjectOfType<GameSession>();
        originalColor = this.GetComponent<SpriteRenderer>().color;
    }

    private void OnMouseOver()
    {
        if (editing) return;
        
        this.GetComponent<SpriteRenderer>().color = Color.magenta;
    }

    private void OnMouseExit()
    {
        if (editing) return;
        
        this.GetComponent<SpriteRenderer>().color = originalColor;
    }


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

    private void OnMouseDown()
    {
        if (!editing)
        {

            this.GetComponent<SpriteRenderer>().color = originalColor;

            session.ActivateEditMove(this.gameObject);
            editing = true;

            coolDownClick = Time.time;

        }
        
    }

}