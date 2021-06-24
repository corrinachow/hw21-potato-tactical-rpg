using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    // Start is called before the first frame update

    public int squareRadius = 5;

    [SerializeField] private Tilemap tilemap;

    // [SerializeField] private Tile hoverTile;

    [SerializeField] private GameObject Player;

    private BoundsInt area;

    private BoundsInt previousArea;

    void Start()
    {

    }

    void DeletePreviousHighlightedTiles()
    {
        TileBase[] tileArray = tilemap.GetTilesBlock(previousArea);

        for (int index = 0; index < tileArray.Length; index++)
        {
            // tilemap.SetTile(tileArray[index].GetTileData().position, null);
        }
    }

    void HighlightCurrentTiles()
    {
        TileBase[] tileArray = tilemap.GetTilesBlock(area);
        for (int index = 0; index < tileArray.Length; index++)
        {
            // tilemap.SetTile(tileArray[index].GetTileData().position, hoverTile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int position = Vector3Int.RoundToInt(Player.transform.position);

        area = new BoundsInt(position, new Vector3Int(squareRadius, squareRadius, 0));


        if (previousArea != null)
        {
            DeletePreviousHighlightedTiles();
        }

        if (area != previousArea)
        {

            HighlightCurrentTiles();

            previousArea = area;
        }
    }
}
