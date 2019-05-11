using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public List<Sprite> Sprites = new List<Sprite>();
    public GameObject TilePrefab;
    public int GridDimension = 8;
    public float Distance = 1.0f;
    private GameObject[,] Grid;

    // Start is called before the first frame update
    void Start()
    {
        Grid = new GameObject[GridDimension, GridDimension];
        InitGrid();
    }

    void InitGrid()
    {
        Vector3 positionOffset = transform.position - new Vector3(GridDimension * Distance / 2.0f, GridDimension * Distance / 2.0f, 0);

        for(int column = 0; column < GridDimension; column++)
            for(int row = 0; row < GridDimension; row++)
            {
                GameObject newTile = Instantiate(TilePrefab);
                SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>();
                renderer.sprite = Sprites[Random.Range(0, Sprites.Count)];
                newTile.transform.parent = transform;
                newTile.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset;
                
                Grid[column, row] = newTile;
            }
    }
}
