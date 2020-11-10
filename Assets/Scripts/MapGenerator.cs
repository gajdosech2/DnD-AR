using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    public GameObject tile;
    public GameObject pillar;
    public GameObject skeleton;
    int ITERATIONS = 16;

    void Start()
    {
        Stack<(int, int)> stack = new Stack<(int, int)>();
        List<(int, int)> tiles = new List<(int, int)>();
        stack.Push((0, 0));
        tiles.Add((0, 0));
        
        for (int i = 0; i < ITERATIONS && stack.Count > 0; i++)
        {
            (int, int) c1 = stack.Pop();
            foreach ((int, int) o in new List<(int, int)> {(-1, 0), (1, 0), (0, -1), (0, 1)})
            {
                (int, int) c2 = (c1.Item1 + o.Item1, c1.Item2 + o.Item2);
                if (Random.Range(0, 2) == 0 && !tiles.Contains(c2))
                {
                    Instantiate(tile, new Vector3(c2.Item1, 0, c2.Item2), Quaternion.identity);
                    stack.Push(c2);
                    tiles.Add(c2);
                    if (Random.Range(0, 2) == 0)
                    {
                        Instantiate(pillar, new Vector3(c2.Item1, 0, c2.Item2), Quaternion.identity);
                    }
                    if (Random.Range(0, 2) == 0)
                    {
                        Instantiate(skeleton, new Vector3(c2.Item1, 0, c2.Item2), Quaternion.identity);
                    }
                }
            }
        }
    }
}
