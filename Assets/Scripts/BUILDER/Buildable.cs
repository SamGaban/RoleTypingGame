using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour
{
    private GameSession session;

    private bool editing = false;

    private float sensitivity = 1.0f;


    private void Start()
    {
        session = FindObjectOfType<GameSession>();
    }

    private void OnMouseDown()
    {
        if (!editing)
        {
            session.ActivateEditMove(this.gameObject);
            editing = true;
        }
        else
        {
            session.DeactivateEditMove();
            editing = false;
        }

    }

    private void Update()
    {
        if (!editing) return;
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        this.gameObject.transform.position = Vector2.Lerp(transform.position, mousePosition, sensitivity * Time.deltaTime);
    }
}
