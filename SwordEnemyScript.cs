using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemyScript : MonoBehaviour
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
    public float knockback;
    public float damage;
    public GameObject sword;
    public float closeColliderRadius;
    public float coolDwnAfterAttack;
    [HideInInspector]public float coolDwnAfterAttackCounter;
    private bool receivingKnockback;
    public int coinsMax;
    public int coinsMin;
    [SerializeField] private LayerMask detectionLayer;
    public float xpGranted;
    private bool isEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        controllerScript = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        controllerScript.enemies.Add(this.gameObject);
        sword.GetComponent<EnemySwordScript>().coolDwn = attackCoolDwn;
        sword.GetComponent<EnemySwordScript>().knockback = knockback;
        sword.GetComponent<EnemySwordScript>().damage = damage;
    }
    private void FixedUpdate()
    {
        if (!isEnabled)
            return;
        hit = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        if (coolDwnAfterAttackCounter > 0)
            coolDwnAfterAttackCounter -= Time.deltaTime;
        if (hit && coolDwnAfterAttackCounter <= 0 && checkIfReachable())
        {
            MoveTowards();
        }
        else if (coolDwnAfterAttackCounter <= 0)
        {
            Patrol();
            if (timeBtwDirChange >= 0)
                timeBtwDirChange -= Time.deltaTime;
        }
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
    private void MoveTowards()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;
        sword.GetComponent<EnemySwordScript>().PointTowardsEnemy(player.transform.position);
        Collider2D hitPlayerClose = Physics2D.OverlapCircle(transform.position, closeColliderRadius, playerLayer);
        if (hitPlayerClose && !receivingKnockback)
        {
            transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            Attack();
            ChangeAnimationState(gameObject.name + "Idle");
            if (sword.GetComponent<EnemySwordScript>().coolDwnCounter > 0)
                sword.GetComponent<EnemySwordScript>().coolDwnCounter -= Time.deltaTime;
        }
        else if(coolDwnAfterAttackCounter <= 0 && !receivingKnockback)
        {
            ChangeAnimationState(gameObject.name + "Walk");
            transform.parent.GetComponent<Rigidbody2D>().velocity = dir * speed;
        }

    }
    private void Attack()
    {
        sword.GetComponent<EnemySwordScript>().Attack();
    }

    private void Patrol()
    {
        sword.GetComponent<EnemySwordScript>().PointInDirection(new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x, transform.parent.GetComponent<Rigidbody2D>().velocity.y));
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
            if (hit.collider.name == "Player")
                return true;
            else
            {
                Debug.Log(hit.collider.name);
                return false;
            }
        }
        else
            return false;
    }
    public void Enable()
    {
        isEnabled = true;
        transform.parent.gameObject.layer = 3;
        gameObject.layer = 3;
        ChangeAnimationState("DemonKnightIdle");
    }

}
