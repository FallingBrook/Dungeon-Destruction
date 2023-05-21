using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objects;

    public LayerMask badLayers;
    public GameObject shop;
    // Start is called before the first frame update
    void Start()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, badLayers);
        if (!hit)
        {
            int num = Random.Range(0, 11);
            if(num < 7 || GameObject.FindWithTag("RoomTemplates").transform.parent.GetComponent<RoomTemplates>().shopSpawned)
            {
                int rand = Random.Range(0, objects.Length);
                GameObject instance = Instantiate(objects[rand], transform.position, Quaternion.identity);
                instance.transform.parent = transform;
            }
            else
            {
                GetComponent<SpawnObjectInPos>().enabled = false;
                GameObject.FindWithTag("RoomTemplates").transform.parent.GetComponent<RoomTemplates>().shopSpawned = true;
                GameObject instance = Instantiate(shop, transform.position, Quaternion.identity);
                instance.transform.parent = transform;
            }
        }
    }
}
    