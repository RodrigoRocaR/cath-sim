using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] blockVariants;

    private int[,,] _level =
    {
        {
            {0, 0, 0},
            {0, 0, 0},
            {0, 0, 0}
        },
        {
            {0, -1, -1},
            {0, 0, 0},
            {-1, -1, -1}
        },
        {
            {-1, -1, -1},
            {-1, 0, -1},
            {-1, -1, -1}
        },
    };
   
    void Start()
    {
        SpawnBlocks();
    }

    
    // void Update()
    // {
    //     
    // }

    private void SpawnBlocks()
    {
        int x = 0, y = 0, z = 0;
        for (int i=0; i<_level.GetLength(0); i++)
        {
            for (int j=0; j<_level.GetLength(1); j++)
            {
                for (int k=0; k<_level.GetLength(2); k++)
                {
                    if (_level[i, j, k] != -1)
                    {
                        Instantiate(blockVariants[_level[i, j, k]], new Vector3(x+k, y+i, z+j), 
                            blockVariants[_level[i, j, k]].transform.rotation);
                    }
                }
            }
        }
    }
}
