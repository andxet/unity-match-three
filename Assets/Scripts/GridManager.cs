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

    public static GridManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Grid = new GameObject[GridDimension, GridDimension];
        InitGrid();
    }

    void InitGrid()
    {
        Vector3 positionOffset = transform.position - new Vector3(GridDimension * Distance / 2.0f, GridDimension * Distance / 2.0f, 0);

        for (int row = 0; row < GridDimension; row++)
            for (int column = 0; column < GridDimension; column++)
            {
                GameObject newTile = Instantiate(TilePrefab);

                List<Sprite> possibleSprites = new List<Sprite>(Sprites);

                //Choose what sprite to use for this cell
                Sprite left1 = GetSpriteAt(column - 1, row);
                Sprite left2 = GetSpriteAt(column - 2, row);
                if (left2 != null && left1 == left2)
                {
                    possibleSprites.Remove(left1);
                }

                Sprite down1 = GetSpriteAt(column, row - 1);
                Sprite down2 = GetSpriteAt(column, row - 2);
                if (down2 != null && down1 == down2)
                {
                    possibleSprites.Remove(down1);
                }

                SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>();
                renderer.sprite = possibleSprites[Random.Range(0, possibleSprites.Count)];

                Tile tile = newTile.AddComponent<Tile>();
                tile.Position = new Vector2Int(column, row);

                newTile.transform.parent = transform;
                newTile.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset;
                
                Grid[column, row] = newTile;
            }
    }

    Sprite GetSpriteAt(int column, int row)
    {
        if (column < 0 || column >= GridDimension
         || row < 0 || row >= GridDimension)
            return null;
        GameObject tile = Grid[column, row];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer.sprite;
    }

    SpriteRenderer GetSpriteRendererAt(int column, int row)
    {
        if (column < 0 || column >= GridDimension
         || row < 0 || row >= GridDimension)
            return null;
        GameObject tile = Grid[column, row];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer;
    }

    public void SwapTiles(Vector2Int tile1Position, Vector2Int tile2Position)
    {
        GameObject tile1 = Grid[tile1Position.x, tile1Position.y];
        SpriteRenderer renderer1 = tile1.GetComponent<SpriteRenderer>();
        
        GameObject tile2 = Grid[tile2Position.x, tile2Position.y];
        SpriteRenderer renderer2 = tile2.GetComponent<SpriteRenderer>();

        Sprite temp = renderer1.sprite;
        renderer1.sprite = renderer2.sprite;
        renderer2.sprite = temp;

        bool changesOccurs = CheckMatches();
        if(!changesOccurs)
        {
            temp = renderer1.sprite;
            renderer1.sprite = renderer2.sprite;
            renderer2.sprite = temp;
        }
        else
        {
            do
            {
                FillHoles();
            } while (CheckMatches());
        }
    }

    bool CheckMatches()
    {
        HashSet<SpriteRenderer> matchedTiles = new HashSet<SpriteRenderer>();
        for (int row = 0; row < GridDimension; row++)
        {
            for (int column = 0; column < GridDimension; column++)
            {
                SpriteRenderer current = GetSpriteRendererAt(column, row);

                List<SpriteRenderer> horizontalMatches = FindColumnMatchForTile(column, row, current.sprite);
                if (horizontalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(horizontalMatches);
                    matchedTiles.Add(current);
                }

                List<SpriteRenderer> verticalMatches = FindRowMatchForTile(column, row, current.sprite);
                if (verticalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(verticalMatches);
                    matchedTiles.Add(current);
                }
            }
        }

        foreach (SpriteRenderer renderer in matchedTiles)
        {
            renderer.sprite = null;
        }
        return matchedTiles.Count > 0;
    }

    List<SpriteRenderer> FindColumnMatchForTile(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = col + 1; i < GridDimension; i++)
        {
            SpriteRenderer nextColumn = GetSpriteRendererAt(i, row);
            if (nextColumn.sprite != sprite)
            {
                break;
            }
            result.Add(nextColumn);
        }
        return result;
    }

    List<SpriteRenderer> FindRowMatchForTile(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = row + 1; i < GridDimension; i++)
        {
            SpriteRenderer nextRow = GetSpriteRendererAt(col, i);
            if (nextRow.sprite != sprite)
            {
                break;
            }
            result.Add(nextRow);
        }
        return result;
    }

    void FillHoles()
    {
        for (int column = 0; column < GridDimension; column++)
            for (int row = 0; row < GridDimension; row++)
            {
                while (GetSpriteRendererAt(column, row).sprite == null)
                {
                    for (int filler = row; filler < GridDimension - 1; filler++)
                    {
                        SpriteRenderer current = GetSpriteRendererAt(column, filler);
                        SpriteRenderer next = GetSpriteRendererAt(column, filler + 1);
                        current.sprite = next.sprite;
                    }
                    SpriteRenderer last = GetSpriteRendererAt(column, GridDimension - 1);
                    last.sprite = Sprites[Random.Range(0, Sprites.Count)];
                }
            }
    }
}
