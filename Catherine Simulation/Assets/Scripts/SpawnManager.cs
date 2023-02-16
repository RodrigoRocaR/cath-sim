using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    public GameObject[] blockVariants;
    private int[,,] _level;
   
    void Start()
    {
        _level = Level.LevelInt;
        CheckScales();
        SpawnBlocks();
    }

    private void CheckScales()
    {
        if (blockVariants.All(block => block.transform.localScale == Level.BlockScaleVector)) return;
        Debug.LogWarning("The scale of one block is not correct");
    }

    private void SpawnBlocks()
    {
        const float x = 0, y = 0.5f, z = 0;
        for (int i=0; i<_level.GetLength(0); i++)
        {
            for (int j=0; j<_level.GetLength(1); j++)
            {
                for (int k=0; k<_level.GetLength(2); k++)
                {
                    if (_level[i, j, k] != -1)
                    {
                        Instantiate(blockVariants[_level[i, j, k]], 
                            new Vector3((x+k)*Level.BlockScale, (y+i)*Level.BlockScale, (z+j)*Level.BlockScale), 
                            blockVariants[_level[i, j, k]].transform.rotation);
                    }
                }
            }
        }
    }
}
