using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraScript : MonoBehaviour
{
    public float maxY;
    public float maxX;
    public float minY;
    public float minX;

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(target.position.x < maxX && target.position.x > minX)
        {
            transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
        }
        if (target.position.y < maxY && target.position.y > minY)
        {
            transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
        }
    }
}
