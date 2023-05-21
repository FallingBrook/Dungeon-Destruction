using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowEnemyScript : MonoBehaviour
{
    private EnemyController controllerScript;
    Collider2D hit;
    private GameObject player;
    public LayerMask playerLayer;

    public float radius;
    public float patrolSpeed;
    public float health;

    private float timeBtwDirChange = 0;
    private Animator anim;
    private string currentState;
    private float timePassed;
    [SerializeField, Range(1f, 0.01f)] private float meleeKnockbackResistance;
    [SerializeField, Range(1f, 0.01f)] private float rifleKnockbackResistance;
    public float attackCoolDwn;
    public float knockback;
    public float damage;
    public GameObject weapon;
    private bool receivingKnockback;
    public int coinsMax;
    public int coinsMin;
    [SerializeField] private LayerMask detectionLayer;
    public float projSpeed;
    public float xpGranted;
    private bool isEnabled = false;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        controllerScript = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        controllerScript.enemies.Add(this.gameObject);
        weapon.GetComponent<EnemyBowScript>().coolDwn = attackCoolDwn;
        weapon.GetComponent<EnemyBowScript>().knockback = knockback;
        weapon.GetComponent<EnemyBowScript>().damage = damage;
    }
    private void FixedUpdate()
    {
        if (!isEnabled)
            return;
        hit = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        if (hit && CheckIfReachable())
        {
            Attack();
            weapon.GetComponent<EnemyBowScript>().PointTowardsEnemy(player.transform.position);
        }
        else
            weapon.GetComponent<EnemyBowScript>().PointInDirection(transform.parent.GetComponent<Rigidbody2D>().velocity);
        Patrol();
        if (timeBtwDirChange >= 0)
            timeBtwDirChange -= Time.deltaTime;
        if (health <= 0)
            Die();
        if (receivingKnockback)
        {
            transform.parent.GetComponent<Rigidbody2D>().isKinematic = false;
        }
        else
        {
            transform.parent.GetComponent<Rigidbody2D>().isKinematic = true;
            transform.parent.GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }
    private void Attack()
    {
        weapon.GetComponent<EnemyBowScript>().speed = projSpeed;
        weapon.GetComponent<EnemyBowScript>().Attack();
    }

    private void Patrol()
    {
        ChangeAnimationState(gameObject.name + "Walk");
        if (timeBtwDirChange <= 0 && !receivingKnockback)
        {
            Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            transform.parent.GetComponent<Rigidbody2D>().velocity = dir * patrolSpeed;
            timeBtwDirChange = Random.Range(0.5f, 3f);
        }

    }
    private void Die()
    {
        controllerScript.enemies.Remove(gameObject);
        GetComponent<SpawnObjectsInRadius>().spawnCoin(Random.Range(coinsMin, coinsMax));
        PlayerPrefs.SetInt("EnemiesKilled", PlayerPrefs.GetInt("EnemiesKilled") + 1);
        GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>().UpdateXp(xpGranted);
        PlayerPrefs.Save();
        Destroy(gameObject);
    }

    public void ReceiveDamage(float damage, Vector2 direction, float knockback, string type)
    {
        if (!isEnabled)
            return;
        if (type == "Melee")
            knockback *= meleeKnockbackResistance;
        else
            knockback *= rifleKnockbackResistance;
        receivingKnockback = true;
        transform.parent.GetComponent<Rigidbody2D>().AddForce(direction.normalized * knockback);
        StartCoroutine(Knockback(direction, knockback));
        health -= damage;
    }

    IEnumerator Knockback(Vector2 direction, float knockback)
    {
        timePassed = 0;
        while (timePassed < 0.15)
        {
            transform.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            transform.parent.GetComponent<Rigidbody2D>().AddForce(direction * knockback);
            timePassed += Time.deltaTime;
            if (timePassed >= 0.15)
            {
                transform.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                receivingKnockback = false;
                transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                timeBtwDirChange = Random.Range(0.5f, 3f);
            }
            yield return null;
        }
    }

    void ChangeAnimationState(string newState)
    {
        //Debug.Log(direction.x);
        // stop same anim from playing
        if (currentState == newState) return;
        // play new anim
        anim.Play(newState);

        // reassign the current state
        currentState = newState;
    }

    bool CheckIfReachable()
    {
        Vector2 dir = (player.transform.position - transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, radius, detectionLayer);
        Debug.DrawRay(transform.position, dir, Color.red);
        if (hit)
        {
            if (hit.collider.name == "Player")
                return true;
            else
                return false;
        }
        else
            return false;
    }
    public void Enable()
    {
        transform.parent.gameObject.layer = 3;
        gameObject.layer = 3;
        GetComponent<CircleCollider2D>().enabled = true;
        ChangeAnimationState("SkeletonArcherIdle");
        isEnabled = true;
    }
}
