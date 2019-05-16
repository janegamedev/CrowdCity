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
    [Range(0, 1)]
    public float ObstaclePercent;

    public float tileSize;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;
    public int seed;

    Coord mapCenter;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        // Generating coords
        allTileCoords = new List<Coord>();
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                allTileCoords.Add(new Coord(i, j));
            }
        }

        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed)); //SEED!
        mapCenter = new Coord((int)mapSize.x / 2, (int)mapSize.y / 2);
        //Create map holder object
        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        // Spawning tiles
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                Vector3 tilePosition = CoordToPosition(i,j);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent)*tileSize;
                newTile.parent = mapHolder;
            }
        }

        // Spawning obstacles
        bool[,] obstacleMap = new bool[(int)mapSize.x,(int)mapSize.y];

        int obstacleCount = (int)(mapSize.x * mapSize.y * ObstaclePercent);
        int currentObstacleCount = 0;

        
        for (int m = 0; m < obstacleCount; m++)
        {
            Coord randomCoord = GetRandomCoords();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;

            if(randomCoord!=mapCenter&& MapIsFollyAccessible(obstacleMap, currentObstacleCount))
            {
                Vector3 obctaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);

                Transform newObctacle = Instantiate(obctaclePrefab, obctaclePosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                newObctacle.parent = mapHolder;
                newObctacle.localScale = Vector3.one *(1-outlinePercent)* tileSize;
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
            
        }
    }

    bool MapIsFollyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue=new Queue<Coord>();
        queue.Enqueue(mapCenter);
        mapFlags[mapCenter.x, mapCenter.y] = true;

        int accessibleTileCount = 1;

        while (queue.Count>0)
        {
            Coord tile = queue.Dequeue();

            for (int i = -1; i <=1; i++)
            {
                for (int j = -1; j <=1; j++)
                {
                    int neighbourX = tile.x + i;
                    int neighbourY = tile.y + j;

                    if (i == 0 || j == 0)
                    {
                        if(neighbourX>=0 && neighbourX < obstacleMap.GetLength(0)&& neighbourY>=0&& neighbourY < obstacleMap.GetLength(1))
                        {
                            if(!mapFlags[neighbourX,neighbourY]&& !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }
        int targetAcessibleTileCount = (int)(mapSize.x*mapSize.y-currentObstacleCount);
        return targetAcessibleTileCount == accessibleTileCount;
    }
    
    Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 * 0.5f + y)*tileSize;
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

        public static bool operator ==(Coord c1, Coord c2)
        {
            return c1.x == c2.x && c1.y == c2.y;
        }
        public static bool operator !=(Coord c1, Coord c2)
        {
            return !(c1 == c2);
        }
    }
}
