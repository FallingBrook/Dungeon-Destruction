using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSellableItemScript : MonoBehaviour
{
    public GameObject[] common;
    public GameObject[] unCommon;
    public GameObject[] Rare;
    public GameObject[] Rarer;
    [HideInInspector] public int price;
    // Start is called before the first frame update
    void Start()
    {
        SpawnItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItem()
    {
        int numb = Random.Range(0, 100);
        if (numb <= 40)
        {
            int n = Random.Range(0, common.Length);
            GameObject instance = Instantiate(common[n], transform.position, Quaternion.identity, transform);
            instance.name = common[n].name;
            price = Random.Range(10, 30);
        }
        else if (numb <= 70)
        {
            int n = Random.Range(0, unCommon.Length);
            GameObject instance = Instantiate(unCommon[n], transform.position, Quaternion.identity, transform);
            instance.name = unCommon[n].name;
            price = Random.Range(30, 50);
        }
        else if (numb <= 90)
        {
            int n = Random.Range(0, Rare.Length);
            GameObject instance = Instantiate(Rare[n], transform.position, Quaternion.identity, transform);
            instance.name = Rare[n].name;
            //instance.GetComponent<SpriteRenderer>().sort
            price = Random.Range(50, 70);
        }
        else if (numb <= 100)
        {
            int n = Random.Range(0, Rarer.Length);
            GameObject instance = Instantiate(Rarer[n], transform.position, Quaternion.identity, transform);
            instance.name = Rarer[n].name;
            price = Random.Range(70, 100);
        }
    }
}
