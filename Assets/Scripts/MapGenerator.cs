using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class MapGenerator : NetworkBehaviour
{
    public GameObject tile;
    public GameObject pillar;
    public GameObject skeleton;
    int ITERATIONS = 16;

    public void SpawnMap()
    {
        Stack<(int, int)> stack = new Stack<(int, int)>();
        List<(int, int)> tiles = new List<(int, int)>();
        stack.Push((0, 0));
        tiles.Add((0, 0));
        bool empty = true;
        
        for (int i = 0; i < ITERATIONS && stack.Count > 0; i++)
        {
            (int, int) c1 = stack.Pop();
            foreach ((int, int) o in new List<(int, int)> {(-1, 0), (1, 0), (0, -1), (0, 1)})
            {
                (int, int) c2 = (c1.Item1 + o.Item1, c1.Item2 + o.Item2);
                if ((Random.Range(0, 2) == 0 || empty) && !tiles.Contains(c2))
                {
                    var tile_object = Instantiate(tile, new Vector3(c2.Item1, 0, c2.Item2), Quaternion.identity);
                    NetworkServer.Spawn(tile_object);
                    empty = false;
                    stack.Push(c2);
                    tiles.Add(c2);
                    if (Random.Range(0, 2) == 0)
                    {
                        var pillar_object = Instantiate(pillar, new Vector3(c2.Item1, 0, c2.Item2), Quaternion.identity);
                        NetworkServer.Spawn(pillar_object);
                    }
                    if (Random.Range(0, 2) == 0)
                    {
                        var skeleton_object = Instantiate(skeleton, new Vector3(c2.Item1, 0, c2.Item2), Quaternion.identity);
                        NetworkServer.Spawn(skeleton_object);
                    }
                }
            }
        }
    }
}
