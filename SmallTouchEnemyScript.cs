using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTouchEnemyScript : MonoBehaviour
{
    private EnemyController controllerScript;
    Collider2D hit;
    private GameObject player;
    public LayerMask playerLayer;

    public float radius;
    public float speed;
    public float patrolSpeed;
    public float health;

    private float timeBtwDirChange = 0;
    private Animator anim;
    private string currentState;
    private float timePassed;
    [SerializeField, Range(1f, 0.01f)] private float meleeKnockbackResistance;
    [SerializeField, Range(1f, 0.01f)] private float rifleKnockbackResistance;
    public float attackCoolDwn;
    private float attackCoolDwnCounter;
    public float knockback;
    public float damage;
    public float closeColliderRadius;
    private bool receivingKnockback;
    public int coinsMax;
    public int coinsMin;
    [SerializeField] private LayerMask detectionLayer;
    public float xpGranted;
    private bool isEnabled = false;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        controllerScript = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        controllerScript.enemies.Add(this.gameObject);
    }

    private void FixedUpdate()
    {
        if (!isEnabled)
            return;
        hit = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        if (hit && checkIfReachable())
        {
            MoveTowards();
        }
        else
        {
            Patrol();
            if (timeBtwDirChange >= 0)
                timeBtwDirChange -= Time.deltaTime;
        }
        if (health <= 0)
            Die();
        if (transform.parent.GetComponent<Rigidbody2D>().velocity.x > 0)
            transform.localScale = new Vector3(1, 1, 0);
        else if (transform.parent.GetComponent<Rigidbody2D>().velocity.x < 0)
            transform.localScale = new Vector3(-1, 1, 0);
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
        Vector2 dir = (player.transform.position - transform.position).normalized;
        Collider2D hitPlayerClose = Physics2D.OverlapCircle(transform.position, closeColliderRadius, playerLayer);
        if (hitPlayerClose && !receivingKnockback)
        {
            if (attackCoolDwnCounter > 0)
                attackCoolDwnCounter -= Time.deltaTime;
            transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Attack();
            ChangeAnimationState(gameObject.name + "Idle");
        }
        else if (!receivingKnockback)
        {
            ChangeAnimationState(gameObject.name + "Walk");
            transform.parent.GetComponent<Rigidbody2D>().velocity = dir * speed;
        }

    }
    private void Attack()
    {
        if(attackCoolDwnCounter <= 0)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            player.GetComponent<PlayerScript>().ReceiveDamage(damage, dir, knockback, "Melee");
            attackCoolDwnCounter = attackCoolDwn;
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
        ChangeAnimationState("SmallDemonIdle");
        isEnabled = true;
    }
}
