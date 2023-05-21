using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public GameObject[] common;
    public GameObject[] unCommon;
    public GameObject[] Rare;
    public GameObject[] Rarer;
    public float radius;
    public int coinsMin;
    public int coinsMax;
    private string currentState;
    public void Open()
    {
        ChangeAnimationState(gameObject.name + "Open");
    }
    public void Spawn()
    {

        int num = 0;
        int count = 0;
        int rand = Random.Range(0, 100);
        if (rand > 90)
            num = 1;
        int spawnGun = Random.Range(0, 10);
        while (count <= num && spawnGun <= 7)
        {
            float numX = Random.Range(-radius, radius);
            float numY = Random.Range(-radius, radius);
            int numb = Random.Range(0, 100);
            if (numb <= 40)
            {
                int n = Random.Range(0, common.Length);
                GameObject instance = Instantiate(common[n], new Vector3(transform.position.x + numX, transform.position.y + numY, 0), Quaternion.identity);
                instance.name = common[n].name;
            }
            else if (numb <= 70)
            {
                int n = Random.Range(0, unCommon.Length);
                GameObject instance = Instantiate(unCommon[n], new Vector3(transform.position.x + numX, transform.position.y + numY, 0), Quaternion.identity);
                instance.name = unCommon[n].name;
            }
            else if (numb <= 90)
            {
                int n = Random.Range(0, Rare.Length);
                GameObject instance = Instantiate(Rare[n], new Vector3(transform.position.x + numX, transform.position.y + numY, 0), Quaternion.identity);
                instance.name = Rare[n].name;
            }
            else if (numb <= 100)
            {
                int n = Random.Range(0, Rarer.Length);
                GameObject instance = Instantiate(Rarer[n], new Vector3(transform.position.x + numX, transform.position.y + numY, 0), Quaternion.identity);
                instance.name = Rarer[n].name;
            }
            count++;
        }
        int number = Random.Range(coinsMin, coinsMax + 1);
        GetComponent<SpawnObjectsInRadius>().spawnCoin(number);
        GetComponent<SpawnObjectsInRadius>().spawnMana(number);
    }

    public void ChangeAnimationState(string newState)
    {
        //Debug.Log(direction.x);
        // stop same anim from playing
        if (currentState == newState) return;
        // play new anim
        transform.GetChild(0).GetComponent<Animator>().Play(newState);

        // reassign the current state
        currentState = newState;
    }
}
