using UnityEngine;
using System.Collections;

public class HexGrid : MonoBehaviour {

    public Transform spawnThis;
    public Transform tile;
    public Transform[] tiles = new Transform[3];

    public int width;
    public int height;
/*
    {
        {0, 0, 0, 1, 1, 2},
        {0, 0, 2, 2, 0, 1},
        {0, 2, 2, 1, 1, 0},
        {2, 1, 1, 1, 2, 0},
    };
 * */

    public int x = 6;
    public int y = 4;

    public float radius = 0.5f;
    public bool useAsInnerCircleRadius = true;

    private float offsetX, offsetY;

    void Start()
    {

        int[,] map = generateMap(x, y);
        float unitLength = (useAsInnerCircleRadius) ? (radius / (Mathf.Sqrt(3) / 2)) : radius;

        offsetX = unitLength * Mathf.Sqrt(3);
        offsetY = unitLength * 0.4f;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                Vector2 hexpos = HexOffset(i - 10, j - 15);
                Vector3 pos = new Vector3(hexpos.x, hexpos.y, 0);
                int type = map[i, j];
                if (type == 0)
                {
                    Instantiate(tiles[0], pos, Quaternion.identity);
                }
                if (type == 1)
                {
                    Instantiate(tiles[1], pos, Quaternion.identity);
                }
                if (type == 2)
                {
                    Instantiate(tiles[2], pos, Quaternion.identity);
                }
            }
        }
    }

    Vector2 HexOffset(int x, int y)
    {
        Vector2 position = Vector2.zero;

        if (y % 2 == 0)
        {
            position.x = x * offsetX;
            position.y = y * offsetY;
        }
        else
        {
            position.x = (x + 0.5f) * offsetX;
            position.y = y * offsetY;
        }

        return position;
    }

    int[,] generateMap(int width, int height)
    {
        int[,] map = new int[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                map[i, j] = Random.Range(0, 3);
            }
        }

        return map;
    }
}
