using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectInPos : MonoBehaviour
{
    public List<Transform> pos;
    public GameObject[] set1;
    public GameObject[] set2;
    public int numSet1;
    public int numSet2;
    private int num1 = 0;
    private int num2 = 0;
    private bool spawned = false;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public GameObject chest;
    private bool chestSpawned = true;
    // Update is called once per frame
    void Update()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position, new Vector2(6.6f, 6), 0, playerLayer);
        if (hit && !spawned)
        {
            Invoke("SpawnObject", 0.5f);
            spawned = true;
        }
        //if (hit)
        //{
        //    Debug.Log(num1 + "Num1");
        //    Debug.Log(numSet1 + "NumSet1");
        //}
        if (!chestSpawned)
        {
            if(GameObject.FindWithTag("EnemyController").GetComponent<EnemyController>().enemies.Count == 0)
            {
                Debug.Log("WHATT");
                int position = Random.Range(0, pos.Count + 1);
                GameObject ob = Instantiate(chest, pos[position].position, Quaternion.identity);
                ob.name = chest.name;
                chestSpawned = true;
                //GetComponent<SpawnObjectInPos>().enabled = false;
            }
        }
    }

    public void SpawnObject()
    {
        while(num1 < numSet1)
        {
            int position = Random.Range(0, pos.Count + 1);
            int ob = Random.Range(0, set1.Length);
            Collider2D hit = Physics2D.OverlapPoint(pos[position].position, groundLayer);
            if (!hit)
            {
                GameObject obj = Instantiate(set1[ob], pos[position].transform.position, Quaternion.identity);
                obj.name = set1[ob].name;
                pos.Remove(pos[position]);
                num1++;
            }
        }
        while (num2 < numSet2)
        {
            int position = Random.Range(0, pos.Count + 1);
            int ob = Random.Range(0, set2.Length);
            Collider2D hit = Physics2D.OverlapPoint(pos[position].position, groundLayer);
            if (hit)
                return;
            GameObject obj = Instantiate(set2[ob], pos[position].position, Quaternion.identity);
            obj.name = set2[ob].name;
            pos.Remove(pos[position]);
            num2++;
            Invoke("ChestFalse", 0.5f);
        }
    }

    private void ChestFalse()
    {
        chestSpawned = false;
    }
}
