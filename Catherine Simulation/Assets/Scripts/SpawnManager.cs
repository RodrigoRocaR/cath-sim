using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    public GameObject[] blockVariants;
    private Level _levelObj = new Level();
    private int[,,] _level;
   
    void Start()
    {
        _level = _levelObj.GetTestLevel();
        SpawnBlocks();
    }

    
    // void Update()
    // {
    //     
    // }

    private void SpawnBlocks()
    {
        Vector3 blockScale = blockVariants[0].transform.localScale;
        float x = 0, y = 0.5f, z = 0;
        for (int i=0; i<_level.GetLength(0); i++)
        {
            for (int j=0; j<_level.GetLength(1); j++)
            {
                for (int k=0; k<_level.GetLength(2); k++)
                {
                    if (_level[i, j, k] != -1)
                    {
                        Instantiate(blockVariants[_level[i, j, k]], 
                            new Vector3((x+k)*blockScale.x, (y+i)*blockScale.y, (z+j)*blockScale.z), 
                            blockVariants[_level[i, j, k]].transform.rotation);
                    }
                }
            }
        }
    }
}
