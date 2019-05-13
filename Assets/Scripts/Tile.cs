using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private static Tile selected;
    private SpriteRenderer Renderer;

    private void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    public void Select()
    {
        Renderer.color = Color.grey;
    }

    public void Unselect()
    {
        Renderer.color = Color.white;
    }

    private void OnMouseDown()
    {
        if(selected != null)
        {
            selected.Unselect();
        }
        selected = this;
        Select();
    }
}
