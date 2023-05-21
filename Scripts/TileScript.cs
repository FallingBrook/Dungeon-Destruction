using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{

    public Sprite[] basicTiles;
    public Sprite[] hurtTiles;
    public Sprite[] damagedTiles;
    // Start is called before the first frame update
    void Start()
    {
        int num = Random.Range(0, 11);
        if(num <= 6)
        {
            int rand = Random.Range(0, basicTiles.Length);
            GetComponent<SpriteRenderer>().sprite = basicTiles[rand];
        }
        else if (num > 6 && num < 10)
        {
            int rand = Random.Range(0, hurtTiles.Length);
            GetComponent<SpriteRenderer>().sprite = hurtTiles[rand];
        }
        else if (num == 10)
        {
            int rand = Random.Range(0, damagedTiles.Length);
            GetComponent<SpriteRenderer>().sprite = damagedTiles[rand];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
