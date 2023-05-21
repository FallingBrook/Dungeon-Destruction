using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBomberEnemyScript : MonoBehaviour
{
    private EnemyController controllerScript;
    Collider2D hit;
    private GameObject player;
    public LayerMask playerLayer;
    private bool followingPlayer;
    private bool nearPlayer;
    public float radius;
    public float explosionDetectionRadius;
    public float speed;

    public float patrolSpeed;
    public float health;

    private float timeBtwDirChange = 0;
    public GameObject explosion;

    public float knockback;
    public float damage;
    [SerializeField, Range(1f, 0.01f)] private float meleeKnockbackResistance;
    [SerializeField, Range(1f, 0.01f)] private float rifleKnockbackResistance;
    private bool receivingKnockback;
    public int coinsMax;
    public int coinsMin;
    [SerializeField] private LayerMask detectionLayer;
    private bool exploded = false;
    public float xpGranted;
    private bool isEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        controllerScript = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        controllerScript.enemies.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled)
            return;
        hit = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        Collider2D closeToPlayer = Physics2D.OverlapCircle(transform.position, explosionDetectionRadius, playerLayer);
        if (closeToPlayer)
            nearPlayer = true;
        if (hit)
            followingPlayer = true;
            if (followingPlayer && !nearPlayer && checkIfReachable())
            {
                MoveTowards();
                GetComponent<Animator>().Play("DemonBomberWalk");
            }
            else if (nearPlayer && checkIfReachable())
            {
                transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                GetComponent<Animator>().Play("DemonBomberExplode");
            }
            else
            {
                Patrol();
                timeBtwDirChange -= Time.deltaTime;
                GetComponent<Animator>().Play("DemonBomberWalk");
            }
            if (transform.parent.GetComponent<Rigidbody2D>().velocity.x > 0)
                transform.localScale = new Vector3(1, 1, 0);
            else if (transform.parent.GetComponent<Rigidbody2D>().velocity.x < 0)
                transform.localScale = new Vector3(-1, 1, 0);

        if (health <= 0 && !exploded)
        {
            Explode();
        }
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
    private void MoveTowards()
    {
        Vector2 pos = transform.position;
        Vector2 dir = (player.transform.position - transform.position).normalized;
        Vector2 dir1 = (player.transform.position - transform.position);
        //if(dir)
        Collider2D hitPlayerClose = Physics2D.OverlapCircle(transform.position, 0.05f, playerLayer);
        if (hitPlayerClose)
            transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        else
            transform.parent.GetComponent<Rigidbody2D>().velocity = dir * speed;

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
        float timePassed = 0;
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
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
                transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            yield return null;
        }
    }

    private void Die()
    {
        controllerScript.enemies.Remove(gameObject);
        transform.parent.GetComponent<SpawnObjectsInRadius>().spawnCoin(Random.Range(coinsMin, coinsMax));
        PlayerPrefs.SetInt("EnemiesKilled", PlayerPrefs.GetInt("EnemiesKilled") + 1);
        GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>().UpdateXp(xpGranted);
        PlayerPrefs.Save();
        Destroy(gameObject);

    }

    public void Explode()
    {
        exploded = true;
        GameObject explostion = Instantiate(explosion, transform.position, Quaternion.identity);
        explostion.GetComponent<ExplosionScript>().knockBack = knockback * 50;
        explostion.GetComponent<ExplosionScript>().damage = damage;
        controllerScript.enemies.Remove(gameObject);
        GetComponent<SpawnObjectsInRadius>().spawnCoin(Random.Range(coinsMin, coinsMax));
        Destroy(gameObject);
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
        }else
            return false;
    }
    public void Enable()
    {
        transform.parent.gameObject.layer = 3;
        gameObject.layer = 3;
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Animator>().Play("DemonBomberWalk");
        isEnabled = true;
        //transform.parent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
