using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectChance : MonoBehaviour
{
    public GameObject[] common;
    public GameObject[] unCommon;
    public GameObject[] Rare;
    public GameObject[] Rarer;

    public LayerMask badLayers;
    public GameObject parent;
    private void Start()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, badLayers);
        if (!hit)
        {
            int num = Random.Range(0, 100);
            if(num <= 40)
            {
                GameObject instance = Instantiate(common[num], transform.position, Quaternion.identity);
                instance.transform.parent = parent.transform;
            }
            else if (num <= 70)
            {
                GameObject instance = Instantiate(unCommon[num], transform.position, Quaternion.identity);
                instance.transform.parent = parent.transform;
            }
            else if (num <= 90)
            {
                GameObject instance = Instantiate(unCommon[num], transform.position, Quaternion.identity);
                instance.transform.parent = parent.transform;
            }
            else if (num <= 100)
            {
                GameObject instance = Instantiate(unCommon[num], transform.position, Quaternion.identity);
                instance.transform.parent = parent.transform;
            }
            GetComponent<SpawnObjectChance>().enabled = false;
        }
    }
}
