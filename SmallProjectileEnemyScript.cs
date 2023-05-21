using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallProjectileEnemyScript : MonoBehaviour
{
    private EnemyController controller;
    Collider2D hit;
    private GameObject player;
    public LayerMask playerLayer;

    public float radius;
    public float patrolSpeed;
    public float health;

    private float timeBtwDirChange = 0;

    public float attackCldwn;
    private float attackCldwnCounter = 0;

    public GameObject projectile;
    private float timePassed;
    public float knockback;
    [SerializeField, Range(1f, 0.01f)] private float meleeKnockbackResistance;
    [SerializeField, Range(1f, 0.01f)] private float rifleKnockbackResistance;
    private bool receivingKnockback;
    public int coinsMax;
    public int coinsMin;
    public int damage;
    [SerializeField] private LayerMask detectionLayer;
    public float xpGranted;
    private bool isEnabled = false;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        controller = GameObject.FindWithTag("EnemyController").GetComponent<EnemyController>();
        controller.enemies.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled)
            return;
        Patrol();
        timeBtwDirChange -= Time.deltaTime;
        attackCldwnCounter -= Time.deltaTime;
        hit = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        if (hit && checkIfReachable())
        {
            Attack();
        }

        if (transform.parent.GetComponent<Rigidbody2D>().velocity.x > 0)
            transform.localScale = new Vector3(1, 1, 0);
        else
            transform.localScale = new Vector3(-1, 1, 0);
        if (health <= 0)
            Die();
    }
    private void FixedUpdate()
    {
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
    private void Die()
    {
        controller.enemies.Remove(gameObject);
        GetComponent<SpawnObjectsInRadius>().spawnCoin(Random.Range(coinsMin, coinsMax));
        PlayerPrefs.SetInt("EnemiesKilled", PlayerPrefs.GetInt("EnemiesKilled") + 1);
        GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>().UpdateXp(xpGranted);
        PlayerPrefs.Save();
        Destroy(gameObject);
    }
    private void Attack()
    {
        Vector2 pos = transform.position;
        Vector2 dir = (player.transform.position - transform.position).normalized;
        if (attackCldwnCounter <= 0)
        {
            GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
            proj.GetComponent<EnemyProjectileScript>().Move(dir);
            proj.GetComponent<EnemyProjectileScript>().knockback = knockback;
            proj.GetComponent<EnemyProjectileScript>().damage = damage;
            attackCldwnCounter = attackCldwn;
        }
    }

    private void Patrol()
    {
        if (timeBtwDirChange <= 0)
        {
            Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            transform.parent.GetComponent<Rigidbody2D>().velocity = dir * patrolSpeed;
            timeBtwDirChange = Random.Range(0.5f, 3f);
        }
    }
    public void ReceiveDamage(float damage, Vector2 direction, float knockback, string type)
    {
        if (!isEnabled)
            return;
        if (type == "Melee")
            knockback *= meleeKnockbackResistance;
        else
            knockback *= rifleKnockbackResistance;
        transform.parent.GetComponent<Rigidbody2D>().AddForce(direction.normalized * knockback);
        StartCoroutine(Knockback(direction, knockback));
        receivingKnockback = true;
        health -= damage;
    }

    IEnumerator Knockback(Vector2 direction, float knockback)
    {
        timePassed = 0;
        while (timePassed < 0.15)
        {
            transform.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            transform.parent.GetComponent<Rigidbody2D>().AddForce(direction * knockback);
            // Code to go left here
            timePassed += Time.deltaTime;
            if (timePassed >= 0.15)
            {
                transform.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                receivingKnockback = false;
            }
            yield return null;
        }
    }
    bool checkIfReachable()
    {
        Vector2 dir = (player.transform.position - transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, radius, detectionLayer);
        Debug.DrawRay(transform.position, dir, Color.red);
        if (hit)
        {
            if (hit.collider.CompareTag("Player"))
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
        GetComponent<Animator>().Play("MageWalk");
        isEnabled = true;
    }
}
