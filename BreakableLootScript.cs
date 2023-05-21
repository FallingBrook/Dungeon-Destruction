using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BreakableLootScript : MonoBehaviour
{
    public int coinsMin;
    public int coinsMax;
    private float health = 2;
    private string currentState;
    public bool explosive;
    public GameObject explosion;
    private bool isEnabled = false;
    public void ReceiveDamage(float damage)
    {
        if(currentState != gameObject.name + "Break" && isEnabled)
        {
            health -= damage;
            if (health <= 0)
            {
                ChangeAnimationState(gameObject.name + "Break");
            }
            else
                ChangeAnimationState(gameObject.name + "Damage");
        }
    }

    public void ToIdle()
    {
        ChangeAnimationState(gameObject.name + "Idle");
    }

    void ChangeAnimationState(string newState)
    {
        //Debug.Log(direction.x);
        // stop same anim from playing
        if (currentState == newState) return;
        // play new anim
        GetComponent<Animator>().Play(newState);

        // reassign the current state
        currentState = newState;
    }

    public void SpawnLoot()
    {
        if (!explosive)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<SpawnObjectsInRadius>().spawnCoin(Random.Range(coinsMin, coinsMax + 1));
            GetComponent<SpawnObjectsInRadius>().spawnMana(Random.Range(coinsMin, coinsMax + 1));
            transform.parent.GetComponent<SortingGroup>().sortingOrder = -1;
            Invoke("Destroy", 10);
        }
        else
        {
            GameObject explostion = Instantiate(explosion, transform.position, Quaternion.identity);
            explostion.GetComponent<ExplosionScript>().knockBack = 3 * 50;
            explostion.GetComponent<ExplosionScript>().damage = 2;
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
    public void Enable()
    {
        gameObject.layer = 13;
        transform.GetComponent<CircleCollider2D>().enabled = true;
        ChangeAnimationState(gameObject.name + "Idle");
        isEnabled = true;
    }
}
