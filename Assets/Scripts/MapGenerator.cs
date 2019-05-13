using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform obctaclePrefab;
    public Vector2 mapSize;

    [Range(0, 1)]
    public float outlinePercent;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;
    public int seed;

    private void Start()
    {
        GenerateMap();
    }

    [ContextMenu("Generate")]
    public void GenerateMap()
    {
        allTileCoords = new List<Coord>();
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                allTileCoords.Add(new Coord(i, j));
            }
        }

        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed)); //SEED!

        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + i, 0, -mapSize.y /2 * 0.5f + j);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent);
                newTile.parent = mapHolder;
            }
        }

        int ObstacleCount = 10;
        for (int m = 0; m < ObstacleCount; m++)
        {
            Coord randomCoord = GetRandomCoords();
            Vector3 obctaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
            Transform newObctacle = Instantiate(obctaclePrefab, obctaclePosition+Vector3.up*.5f, Quaternion.identity) as Transform;
            newObctacle.parent = mapHolder;
        }
    }
    
    Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 * 0.5f + y);
    }

    public Coord GetRandomCoords()
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }
}
