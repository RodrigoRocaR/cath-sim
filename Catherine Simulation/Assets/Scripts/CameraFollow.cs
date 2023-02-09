using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    private Vector3 _offset = new Vector3(0, 4, -6.5f);
    private Vector3 _offsetAngle = new Vector3(0, 0, 0);
    
    void Start()
    {
        
    }


    void Update()
    {
        transform.position = target.transform.position + _offset;
        transform.Rotate(_offsetAngle);
    }
}
